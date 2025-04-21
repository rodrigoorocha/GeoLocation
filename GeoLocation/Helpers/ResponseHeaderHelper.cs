using Microsoft.AspNetCore.Http;

namespace GeoLocation.Helpers;

public static class ResponseHeaderHelper
{
    public static void SetStandardHeaders(HttpResponse response, string requestId)
    {
        if (!response.HasStarted) // Ensure headers are not set after the response has started
        {
            //response.Headers["Content-Type"] = "application/json; charset=utf-8";
            response.Headers["Cache-Control"] = "No-store";
            response.Headers["X-Request-Id"] = requestId;
        }
    }
}