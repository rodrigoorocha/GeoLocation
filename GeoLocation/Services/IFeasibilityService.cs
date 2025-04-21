using GeoLocation.Models;
using System.Collections.Generic;

namespace GeoLocation.Services
{
    public interface IFeasibilityService
    {
        List<FeasibilityService.ResourceWithDistance> GetResourcesWithinRadius(double latitude, double longitude, int radiusMeters);
    }
}
