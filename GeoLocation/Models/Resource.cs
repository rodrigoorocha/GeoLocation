namespace GeoLocation.Models;

public class GeometryPoint
{
    public double ?X { get; set; }
    public double ?Y { get; set; }
    public double ?Z { get; set; }
}

public class Resource
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string GeometryType { get; set; }
    public string SpatialRef { get; set; }
    public string Href { get; set; }
    public List<GeometryPoint> Geometry { get; set; }
}