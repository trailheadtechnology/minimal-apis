using class_library;
using System.Diagnostics;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, WeatherForecastSerializerContext.Default);
});

var app = builder.Build();

var Summaries = new string[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var todosApi = app.MapGroup("/weatherforecast");
todosApi.MapGet("/", () =>
{
    Debug.WriteLine($"Minimal API memory used: {Process.GetCurrentProcess().WorkingSet64 / (1024.0 * 1024.0):0.0} MB");

    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    }).ToArray();
});

app.Run();