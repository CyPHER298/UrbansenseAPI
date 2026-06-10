using UrbansenseAPI.Domain.Models;

namespace UrbansenseAPI.Infrastructure.Persistence;

public class WeatherStore
{
    private readonly List<Weather> _history = [];
    private readonly Lock _lock = new();
    private const int MaxEntries = 500;

    public void Add(Weather weather)
    {
        lock (_lock)
        {
            _history.Add(weather);
            if (_history.Count > MaxEntries)
                _history.RemoveAt(0);
        }
    }

    public IReadOnlyList<Weather> GetSince(DateTime since)
    {
        lock (_lock)
            return _history.Where(w => w.RecordedAt >= since).ToList().AsReadOnly();
    }

    public IReadOnlyList<Weather> GetHeavyRainSince(double thresholdMm, DateTime since)
    {
        lock (_lock)
            return _history
                .Where(w => w.RecordedAt >= since && w.IsHeavyRain(thresholdMm))
                .GroupBy(w => w.City)
                .Select(g => g.OrderByDescending(w => w.RecordedAt).First())
                .ToList()
                .AsReadOnly();
    }

    public double? AvgRainForCityHour(string city, int hour, DateTime since)
    {
        lock (_lock)
        {
            var events = _history
                .Where(w => w.City.Equals(city, StringComparison.OrdinalIgnoreCase)
                         && w.RecordedAt >= since
                         && w.RecordedAt.Hour == hour
                         && w.RainMm > 0)
                .ToList();

            return events.Any() ? events.Average(w => w.RainMm ?? 0) : null;
        }
    }

    public int CountRainEventsByCity(string city, DateTime since)
    {
        lock (_lock)
            return _history.Count(w =>
                w.City.Equals(city, StringComparison.OrdinalIgnoreCase)
                && w.RecordedAt >= since
                && w.RainMm > 0);
    }
}
