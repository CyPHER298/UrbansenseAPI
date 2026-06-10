using UrbansenseAPI.Domain.Enums;
using UrbansenseAPI.Domain.Models;

namespace UrbansenseAPI.Infrastructure.Persistence;

public class AlertStore
{
    private readonly List<Alert> _alerts = [];
    private readonly Lock _lock = new();
    private long _nextId = 1;

    public void Add(Alert alert)
    {
        lock (_lock)
        {
            alert.id = _nextId++;
            _alerts.Add(alert);
        }
    }

    public void DeactivateExpired()
    {
        lock (_lock)
        {
            var now = DateTime.UtcNow;
            foreach (var a in _alerts.Where(a => a.validUntil <= now))
                a.active = false;
        }
    }

    public IReadOnlyList<Alert> GetActiveByCity(string city)
    {
        lock (_lock)
        {
            var now = DateTime.UtcNow;
            return _alerts
                .Where(a => a.active
                         && a.city.Equals(city, StringComparison.OrdinalIgnoreCase)
                         && a.validUntil > now)
                .OrderByDescending(a => a.severity)
                .ToList()
                .AsReadOnly();
        }
    }

    public IReadOnlyList<Alert> GetActiveByType(AlertType type)
    {
        lock (_lock)
        {
            var now = DateTime.UtcNow;
            return _alerts
                .Where(a => a.active && a.type == type && a.validUntil > now)
                .ToList()
                .AsReadOnly();
        }
    }
}
