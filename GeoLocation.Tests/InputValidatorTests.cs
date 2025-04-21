using GeoLocation.Helpers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class InputValidatorTests
{
    [Theory]
    [InlineData("-22.91015", "-43.1829", "100", false)] // less than 5 decimals
    [InlineData("-22.910159", "-43.182978", "100", true)] // valid
    [InlineData("-91.00000", "-43.182978", "100", false)] // latitude out of range
    [InlineData("-22.910159", "-181.00000", "100", false)] // longitude out of range
    [InlineData("-22.910159", "-43.182978", "9", false)] // radius too small
    [InlineData("-22.910159", "-43.182978", "1001", false)] // radius too large
    [InlineData("-22.910159", "-43.182978", "100", true)] // valid
    [InlineData("", "-43.182978", "100", false)] // latitude empty
    [InlineData("-22.910159", "", "100", false)] // longitude empty
    [InlineData("-22.910159", "-43.182978", "", false)] // radius empty
    public void Validate_CoversAllCases(string lat, string lon, string rad, bool expectedValid)
    {
        var result = InputValidator.Validate(lat, lon, rad, out double outLat, out double outLon, out int outRad, out IActionResult errorResult);
        Assert.Equal(expectedValid, result);
        if (!expectedValid)
            Assert.NotNull(errorResult);
    }

    [Fact]
    public void Validate_ValidatesDecimalPlaces()
    {
        var valid = InputValidator.Validate("-22.12345", "-43.12345", "100", out _, out _, out _, out _);
        Assert.True(valid);
        var invalid = InputValidator.Validate("-22.1", "-43.12345", "100", out _, out _, out _, out _);
        Assert.False(invalid);
    }
}