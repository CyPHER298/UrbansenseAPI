using UrbansenseAPI.DTOs;

namespace UrbansenseAPI.Domain.Services;

public interface IWeatherService
{
    Task<WeatherResponse> GetCurrentWeatherAsync(string city, double lat, double lon);
    Task<RainRiskResponse> AnalyzeRainRiskAsync(string city, int hour);
    Task<List<WeatherResponse>> GetHeavyRainAreasAsync(double thresholdMm = 20.0);
}
