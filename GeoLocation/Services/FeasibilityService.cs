using GeoLocation.Models;
using System.Text.Json;

namespace GeoLocation.Services;

public class FeasibilityService : IFeasibilityService
{
    private readonly List<Resource> _resources;

    public FeasibilityService(List<Resource> resources)
    {
        _resources = resources ?? new List<Resource>();
    }

    public class ResourceWithDistance
    {
        public Resource Resource { get; set; }
        public double Distance { get; set; }
    }

    public List<ResourceWithDistance> GetResourcesWithinRadius(double latitude, double longitude, int radiusMeters)
    {
        if (latitude < -90 || latitude > 90)
        {
            throw new ArgumentException("latitude must be between -90 and 90");
        }
        if (longitude < -180 || longitude > 180)
        {
            throw new ArgumentException("longitude must be between -180 and 180");
        }
        if (radiusMeters < 10 || radiusMeters > 1000)
        {
            throw new ArgumentException("radius must be between 10 and 1000 (meters)");
        }

        List<ResourceWithDistance> result = new List<ResourceWithDistance>();
        int i = 0;
        while (i < _resources.Count)
        {
            Resource resource = _resources[i];
            if (resource.Geometry != null)
            {
                if (resource.Geometry.Count > 0)
                {
                    GeometryPoint point = resource.Geometry[0];
                    if (!(point.X == 0 && point.Y == 0))
                    {
                        double distance = HaversineDistance(latitude, longitude, point.X, point.Y);
                        if (distance <= radiusMeters)
                        {
                            ResourceWithDistance rwd = new ResourceWithDistance();
                            rwd.Resource = resource;
                            rwd.Distance = distance;
                            result.Add(rwd);
                        }
                    }
                }
            }
            i = i + 1;
        }
        return result;
    }

    public static double HaversineDistance(double startLatitude, double startLongitude, double? endLatitude, double? endLongitude)
    {
        if (!endLatitude.HasValue || !endLongitude.HasValue)
        {
            return double.MaxValue;
        }

        const double EarthRadiusMeters = 6371000; // Raio da Terra em metros

        // Converte graus para radianos
        double startLatRad = DegreesToRadians(startLatitude);
        double startLonRad = DegreesToRadians(startLongitude);
        double endLatRad = DegreesToRadians(endLatitude.Value);
        double endLonRad = DegreesToRadians(endLongitude.Value);

        // Diferenças
        double deltaLat = endLatRad - startLatRad;
        double deltaLon = endLonRad - startLonRad;

        // Fórmula de Haversine
        double a = Math.Pow(Math.Sin(deltaLat / 2), 2) +
                   Math.Cos(startLatRad) * Math.Cos(endLatRad) *
                   Math.Pow(Math.Sin(deltaLon / 2), 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadiusMeters * c;
    }

    private static double DegreesToRadians(double deg) => deg * (Math.PI / 180);
}