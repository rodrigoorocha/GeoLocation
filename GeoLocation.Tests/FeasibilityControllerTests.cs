using GeoLocation.Controllers;
using GeoLocation.Models;
using GeoLocation.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;
using System.Linq;

public class FeasibilityControllerTests
{
    private FeasibilityService GetTestService()
    {
        var controller2 = new FeasibilityController(new FeasibilityService("dataset.json"));

        var resources = new List<Resource>
        {
            new Resource
            {
                Id = 1,
                Name = "Test1",
                GeometryType = "Point",
                SpatialRef = "WGS84",
                Href = "url1",
                Geometry = new List<GeometryPoint> { new GeometryPoint { X = -43.182978, Y = -22.910159, Z = 0 } }
            },
            new Resource
            {
                Id = 2,
                Name = "Test2",
                GeometryType = "Point",
                SpatialRef = "WGS84",
                Href = "url2",
                Geometry = new List<GeometryPoint> { new GeometryPoint { X = -43.183000, Y = -22.910200, Z = 0 } }
            }
        };
        return new FeasibilityService(resources);
    }

    [Fact]
    public void Get_ReturnsOk_WhenResourcesFound()
    {
        var controller = new FeasibilityController(GetTestService());
        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext();
        var result = controller.Get("-22.910159", "-43.182978", "100");
        var ok = Assert.IsType<OkObjectResult>(result);
        var list = Assert.IsAssignableFrom<IEnumerable<object>>(ok.Value);
        Assert.NotNull(list);
        Assert.NotEmpty(list);
    }

    [Fact]
    public void Get_ReturnsOk_EmptyList_WhenNoResourcesFound()
    {
        var controller = new FeasibilityController(GetTestService());
        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext();
        var result = controller.Get("0.00000", "0.00000", "10");
        var ok = Assert.IsType<OkObjectResult>(result);
        var list = Assert.IsAssignableFrom<IEnumerable<object>>(ok.Value);
        Assert.Empty(list);
    }

    [Theory]
    [InlineData("", "-43.182978", "100", "latitude is mandatory")]
    [InlineData("-22.910159", "", "100", "longitude is mandatory")]
    [InlineData("-22.910159", "-43.182978", "", "radius is mandatory")]
    [InlineData("-91.00000", "-43.182978", "100", "latitude must be a float with at least 5 decimal places between -90 and 90")]
    [InlineData("-22.910159", "-181.00000", "100", "longitude must be a float with at least 5 decimal places between -180 and 180")]
    [InlineData("-22.910159", "-43.182978", "9", "radius must be an integer between 10 and 1000 (meters)")]
    public void Get_ReturnsBadRequest_OnValidationError(string lat, string lon, string rad, string expectedMsg)
    {
        var controller = new FeasibilityController(GetTestService());
        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext();
        var result = controller.Get(lat, lon, rad);
        var bad = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains(expectedMsg, bad.Value?.ToString() ?? string.Empty);
    }

    private class FailingFeasibilityService : FeasibilityService
    {
        public FailingFeasibilityService() : base(new List<Resource>())
        {
        }

        public new List<(Resource resource, double distance)> GetResourcesWithinRadius(double latitude, double longitude, int radiusMeters)
        {
            throw new System.Exception();
        }
    }

    [Fact]
    public void Get_ReturnsInternalServerError_OnException()
    {
        var controller = new FeasibilityController(new FailingFeasibilityService());
        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext();
        var result = controller.Get("-22.910159", "-43.182978", "100");
        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, obj.StatusCode);
        Assert.Contains("internal server error", obj.Value?.ToString() ?? string.Empty);
    }
}