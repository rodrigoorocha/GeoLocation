using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace GeoLocation.Helpers;

public static class InputValidator
{
    public static bool Validate(string latitude, string longitude, string radius, out double lat, out double lon, out int rad, out IActionResult? errorResult)
    {
        lat = 0;
        lon = 0;
        rad = 0;
        errorResult = null;

        
        if (latitude == null || latitude.Trim() == "")
        {
            errorResult = BadRequest("latitude is mandatory");
            return false;
        }
        if (longitude == null || longitude.Trim() == "")
        {
            errorResult = BadRequest("longitude is mandatory");
            return false;
        }
        if (radius == null || radius.Trim() == "")
        {
            errorResult = BadRequest("radius is mandatory");
            return false;
        }

        
        bool latOk = double.TryParse(latitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lat);
        if (!latOk)
        {
            errorResult = BadRequest("latitude must be a valid number");
            return false;
        }

        
        bool lonOk = double.TryParse(longitude, NumberStyles.Float, CultureInfo.InvariantCulture, out lon);
        if (!lonOk)
        {
            errorResult = BadRequest("longitude must be a valid number");
            return false;
        }

        
        bool radOk = int.TryParse(radius, out rad);
        if (!radOk)
        {
            errorResult = BadRequest("radius must be a valid integer");
            return false;
        }

        return true;
    }

    private static IActionResult BadRequest(string message)
    {
        return new BadRequestObjectResult(new
        {
            code = "400",
            reason = message.Contains("mandatory") ? "empty field" : "invalid field",
            message,
            status = "bad request",
            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        });
    }
}
