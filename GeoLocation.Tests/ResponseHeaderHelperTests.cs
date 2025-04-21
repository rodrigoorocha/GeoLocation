using GeoLocation.Helpers;
using Microsoft.AspNetCore.Http;
using Xunit;

public class ResponseHeaderHelperTests
{
    [Fact]
    public void SetStandardHeaders_SetsAllHeadersCorrectly()
    {
        var context = new DefaultHttpContext();
        var response = context.Response;
        var requestId = "test-uuid";
        ResponseHeaderHelper.SetStandardHeaders(response, requestId);
        Assert.Equal("application/json; charset=utf-8", response.Headers["Content-Type"]);
        Assert.Equal("No-store", response.Headers["Cache-Control"]);
        Assert.Equal(requestId, response.Headers["X-Request-Id"]);
    }
}
