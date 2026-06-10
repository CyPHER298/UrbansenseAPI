namespace UrbansenseAPI.DTOs;

public record RainRiskResponse(
    string City,
    int Hour,
    double AvgRainMm,
    int HistoricalEvents,
    double RiskScore,
    string Level
);
