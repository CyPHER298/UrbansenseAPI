using UrbansenseAPI.Domain.Enums;

namespace UrbansenseAPI.Domain.Models;

public record Weather(
    long? Id,
    string City,
    double Latitude,
    double Longitude,
    double? Temperature,
    double? FeelsLike,
    int? Humidity,
    double? WindSpeed,
    double? RainMm,
    int? UvIndex,
    WeatherCondition Condition,
    string Description,
    DateTime RecordedAt
)
{
    public bool IsHeavyRain(double thresholdMm) => RainMm.HasValue && RainMm >= thresholdMm;
    public bool IsStorm() => Condition == WeatherCondition.Storm;
    public bool HasHighUv(int threshold) => UvIndex.HasValue && UvIndex >= threshold;
}

