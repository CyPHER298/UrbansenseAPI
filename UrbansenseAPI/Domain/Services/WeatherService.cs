using UrbansenseAPI.Domain.Exceptions;
using UrbansenseAPI.Domain.Models;
using UrbansenseAPI.DTOs;
using UrbansenseAPI.Infrastructure.External;
using UrbansenseAPI.Infrastructure.Persistence;

namespace UrbansenseAPI.Domain.Services;

public class WeatherService(IOpenWeatherClient openWeatherClient, WeatherStore store) : IWeatherService
{
    public async Task<WeatherResponse> GetCurrentWeatherAsync(string city, double lat, double lon)
    {
        try
        {
            var weather = await openWeatherClient.FetchCurrentAsync(city, lat, lon);
            store.Add(weather);
            return ToResponse(weather);
        }
        catch (DomainException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ExternalApiException("OpenWeather", ex.Message);
        }
    }

    public Task<RainRiskResponse> AnalyzeRainRiskAsync(string city, int hour)
    {
        var since      = DateTime.UtcNow.AddDays(-30);
        var avgRain    = store.AvgRainForCityHour(city, hour, since);
        var eventCount = store.CountRainEventsByCity(city, since);

        var risk = RainRisk.Calculate(city, hour, avgRain, eventCount);

        return Task.FromResult(new RainRiskResponse(
            risk.City, risk.Hour, risk.AvgRainMm,
            risk.HistoricalEvents, risk.RiskScore, risk.Level.ToString()
        ));
    }

    public Task<List<WeatherResponse>> GetHeavyRainAreasAsync(double thresholdMm = 20.0)
    {
        var since  = DateTime.UtcNow.AddHours(-1);
        var result = store.GetHeavyRainSince(thresholdMm, since)
                         .Select(ToResponse)
                         .ToList();
        return Task.FromResult(result);
    }

    private static WeatherResponse ToResponse(Weather w) => new(
        w.City, w.Latitude, w.Longitude, w.Temperature, w.FeelsLike,
        w.Humidity, w.WindSpeed, w.RainMm, w.UvIndex,
        w.Condition.ToString(), w.Description, w.RecordedAt
    );
}
