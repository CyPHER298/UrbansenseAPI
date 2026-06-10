using UrbansenseAPI.Domain.Enums;

namespace UrbansenseAPI.Domain.Models;

public record RainRisk(
    string City,
    int Hour,
    double AvgRainMm,
    int HistoricalEvents,
    double RiskScore,
    RiskLevel Level
)
{
    public static RainRisk Calculate(string city, int hour, double? avgRain, int events)
    {
        double rainScore = avgRain.HasValue ? Math.Min(avgRain.Value / 20.0, 1.0) : 0;
        double freqScore = Math.Min(events / 15.0, 1.0);
        double score = (rainScore * 0.6) + (freqScore * 0.4);
        RiskLevel level = score >= 0.7 ? RiskLevel.High : score >= 0.4 ? RiskLevel.Medium : RiskLevel.Low;
        return new RainRisk(city, hour, avgRain ?? 0.0, events, score, level);
    }
}

