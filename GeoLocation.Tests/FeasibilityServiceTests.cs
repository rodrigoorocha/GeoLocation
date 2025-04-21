using GeoLocation.Models;
using GeoLocation.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

public class FeasibilityServiceTests
{
    private readonly string _testDatasetPath = "test_dataset.json";

    private void CreateTestDataset()
    {
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
            },
            new Resource
            {
                Id = 3,
                Name = "Far",
                GeometryType = "Point",
                SpatialRef = "WGS84",
                Href = "url3",
                Geometry = new List<GeometryPoint> { new GeometryPoint { X = -43.000000, Y = -22.000000, Z = 0 } }
            }
        };
        File.WriteAllText(_testDatasetPath, System.Text.Json.JsonSerializer.Serialize(resources));
    }

    [Fact]
    public void GetResourcesWithinRadius_ReturnsCorrectResources()
    {
        CreateTestDataset();
        var service = new FeasibilityService(_testDatasetPath);
        var results = service.GetResourcesWithinRadius(-22.910159, -43.182978, 100);
        Assert.Contains(results, r => r.resource.Name == "Test1");
        Assert.Contains(results, r => r.resource.Name == "Test2");
        Assert.DoesNotContain(results, r => r.resource.Name == "Far");
    }

    [Fact]
    public void HaversineDistance_ReturnsExpectedDistance()
    {
        double d = FeasibilityService.HaversineDistance(-22.910159, -43.182978, -22.910159, -43.182978);
        Assert.Equal(0, d, 2);
        double d2 = FeasibilityService.HaversineDistance(-22.910159, -43.182978, -22.910200, -43.183000);
        Assert.True(d2 > 0);
    }
}
