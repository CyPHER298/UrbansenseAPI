using UrbansenseAPI.Domain.Enums;
using UrbansenseAPI.Domain.Models;
using UrbansenseAPI.DTOs;
using UrbansenseAPI.Infrastructure.Persistence;

namespace UrbansenseAPI.Domain.Services;

public class AlertService(IWeatherService weatherService, AlertStore store, ILogger<AlertService> logger) : IAlertService
{
    private static readonly (string City, double Lat, double Lon)[] MonitoredCities =
    [
        ("São Paulo",   -23.5505, -46.6333),
        ("Guarulhos",   -23.4538, -46.5333),
        ("Osasco",      -23.5322, -46.7919),
        ("Santo André", -23.6639, -46.5383),
        ("Campinas",    -22.9068, -47.0626)
    ];

    public Task<List<AlertResponse>> GetActiveByCityAsync(string city)
    {
        var result = store.GetActiveByCity(city).Select(ToResponse).ToList();
        return Task.FromResult(result);
    }

    public Task<List<AlertResponse>> GetByTypeAsync(string type)
    {
        if (!Enum.TryParse<AlertType>(type, ignoreCase: true, out var alertType))
            return Task.FromResult(new List<AlertResponse>());

        var result = store.GetActiveByType(alertType).Select(ToResponse).ToList();
        return Task.FromResult(result);
    }

    public async Task EvaluateAlertsAsync()
    {
        store.DeactivateExpired();

        foreach (var (city, lat, lon) in MonitoredCities)
        {
            try
            {
                var weather   = await weatherService.GetCurrentWeatherAsync(city, lat, lon);
                var newAlerts = ApplyBusinessRules(weather);
                foreach (var alert in newAlerts)
                    store.Add(alert);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Falha ao avaliar alertas para {City}", city);
            }
        }
    }

    private static List<Alert> ApplyBusinessRules(WeatherResponse w)
    {
        var alerts = new List<Alert>();
        var now    = DateTime.UtcNow;

        if ((w.RainMm ?? 0.0) >= 20.0)
            alerts.Add(new Alert
            {
                city = w.City, type = AlertType.Flooding,
                severity  = (w.RainMm ?? 0.0) >= 40.0 ? Severity.Critical : Severity.High,
                message   = $"Risco de alagamento em {w.City}. Chuva acumulada: {w.RainMm:F1}mm/h.",
                latitude  = w.Latitude, longitude = w.Longitude,
                validFrom = now, validUntil = now.AddHours(2), active = true
            });
        else if ((w.RainMm ?? 0.0) >= 10.0)
            alerts.Add(new Alert
            {
                city = w.City, type = AlertType.HeavyRain,
                severity  = Severity.Medium,
                message   = $"Chuva forte em {w.City}. Precipitação: {w.RainMm:F1}mm/h.",
                latitude  = w.Latitude, longitude = w.Longitude,
                validFrom = now, validUntil = now.AddHours(1), active = true
            });

        if ((w.UvIndex ?? 0) >= 8)
            alerts.Add(new Alert
            {
                city = w.City, type = AlertType.HighUv,
                severity  = (w.UvIndex ?? 0) >= 11 ? Severity.High : Severity.Medium,
                message   = $"Índice UV elevado em {w.City}: {w.UvIndex}. Use protetor solar FPS 30+.",
                latitude  = w.Latitude, longitude = w.Longitude,
                validFrom = now, validUntil = now.AddHours(4), active = true
            });

        if (w.Condition == "Storm")
            alerts.Add(new Alert
            {
                city = w.City, type = AlertType.HeavyRain,
                severity  = Severity.Critical,
                message   = $"Tempestade em {w.City}. Evite áreas abertas e locais alagáveis.",
                latitude  = w.Latitude, longitude = w.Longitude,
                validFrom = now, validUntil = now.AddHours(3), active = true
            });

        return alerts;
    }

    private static AlertResponse ToResponse(Alert a) => new(
        a.id, a.city, a.type.ToString(), a.severity.ToString(),
        a.message, a.region, a.latitude, a.longitude,
        a.validFrom, a.validUntil, a.active
    );
}
