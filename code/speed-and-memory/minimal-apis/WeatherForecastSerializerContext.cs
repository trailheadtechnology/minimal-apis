using class_library;
using System.Text.Json.Serialization;

[JsonSerializable(typeof(WeatherForecast[]))]
internal partial class WeatherForecastSerializerContext : JsonSerializerContext
{

}
