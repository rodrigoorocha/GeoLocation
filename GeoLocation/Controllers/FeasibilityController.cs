using GeoLocation.Helpers;
using GeoLocation.Models;
using GeoLocation.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Resources;

namespace GeoLocation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeasibilityController : ControllerBase
{
    private readonly IFeasibilityService _service;
    private readonly string _requestId;

    public FeasibilityController(IFeasibilityService service)
    {
        _service = service;
        _requestId = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Returns resources within a given radius from a point.
    /// </summary>
    [HttpGet("radius-search")]
    public virtual IActionResult Get([FromQuery] string latitude, [FromQuery] string longitude, [FromQuery] string radius)
    {
        if (!InputValidator.Validate(latitude, longitude, radius, out double lat, out double lon, out int rad, out IActionResult errorResult))
            return errorResult;
        try
        {
            var found = _service.GetResourcesWithinRadius(lat, lon, rad)
                .Select(r => new
                {
                    id = r.Resource.Id,
                    nome = r.Resource.Name,
                    latitude = r.Resource.Geometry[0].Y,
                    longitude = r.Resource.Geometry[0].X,
                    radius = Math.Round(r.Distance, 2)
                })
                .ToList();

            HttpContext.Response.OnStarting(() =>
            {
                ResponseHeaderHelper.SetStandardHeaders(Response, _requestId);
                return Task.CompletedTask;
            });

            // Return 200 with an empty array if no resources are found
            if (found == null || !found.Any())
            {
                return Ok(new object[0]);
            }

            return Ok(found);
        }
        catch
        {
            return StatusCode(500, new
            {
                code = "500",
                reason = "internal server error",
                message = "general fail",
                status = "internal server error",
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            });
        }
    }
}