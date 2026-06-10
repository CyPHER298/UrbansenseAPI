using System.Text.Json.Serialization;
using UrbansenseAPI.Domain.Enums;
using UrbansenseAPI.Domain.Models;

namespace UrbansenseAPI.Infrastructure.External;

public class OpenWeatherClient(HttpClient httpClient, IConfiguration configuration) : IOpenWeatherClient
{
    private readonly string _apiKey = configuration["OpenWeather:ApiKey"]
        ?? throw new InvalidOperationException("OpenWeather:ApiKey não configurado.");

    public async Task<Weather> FetchCurrentAsync(string city, double lat, double lon)
    {
        var url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={_apiKey}&units=metric&lang=pt_br";
        var response = await httpClient.GetFromJsonAsync<OWResponse>(url)
            ?? throw new InvalidOperationException("Resposta vazia da API OpenWeather.");

        var conditionStr = response.Weather?.FirstOrDefault()?.Main ?? "";
        var condition = conditionStr.ToUpper() switch
        {
            "RAIN" or "DRIZZLE" => WeatherCondition.Rain,
            "THUNDERSTORM"      => WeatherCondition.Storm,
            "CLEAR"             => WeatherCondition.Clear,
            "CLOUDS"            => WeatherCondition.Cloudy,
            _                   => WeatherCondition.Other
        };

        return new Weather(
            Id: null,
            City: city,
            Latitude: lat,
            Longitude: lon,
            Temperature: response.Main?.Temp,
            FeelsLike: response.Main?.FeelsLike,
            Humidity: response.Main?.Humidity,
            WindSpeed: response.Wind?.Speed,
            RainMm: response.Rain?.OneHour ?? 0.0,
            UvIndex: null,
            Condition: condition,
            Description: response.Weather?.FirstOrDefault()?.Description ?? "",
            RecordedAt: DateTime.UtcNow
        );
    }

    private record OWResponse(
        [property: JsonPropertyName("main")]    OWMain?    Main,
        [property: JsonPropertyName("wind")]    OWWind?    Wind,
        [property: JsonPropertyName("rain")]    OWRain?    Rain,
        [property: JsonPropertyName("weather")] List<OWWeatherDesc>? Weather
    );

    private record OWMain(
        [property: JsonPropertyName("temp")]       double  Temp,
        [property: JsonPropertyName("feels_like")] double  FeelsLike,
        [property: JsonPropertyName("humidity")]   int     Humidity
    );

    private record OWWind(
        [property: JsonPropertyName("speed")] double Speed
    );

    private record OWRain(
        [property: JsonPropertyName("1h")] double? OneHour
    );

    private record OWWeatherDesc(
        [property: JsonPropertyName("main")]        string Main,
        [property: JsonPropertyName("description")] string Description
    );
}
