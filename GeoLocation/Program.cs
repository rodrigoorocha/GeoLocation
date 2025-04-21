using GeoLocation.Models;
using GeoLocation.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to use a different port
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenLocalhost(5000); // HTTP
//    options.ListenLocalhost(5001, listenOptions => listenOptions.UseHttps()); // HTTPS
//});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IFeasibilityService, FeasibilityService>(op =>
{
    var file = File.ReadAllText("dataset.json");
    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    var objs = JsonSerializer.Deserialize<List<Resource>>(file, options);
    return new FeasibilityService(objs);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "GeoLocation API V1");
        o.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();