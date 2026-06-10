using UrbansenseAPI.DTOs;

namespace UrbansenseAPI.Domain.Services;

public interface IAlertService
{
    Task<List<AlertResponse>> GetActiveByCityAsync(string city);
    Task<List<AlertResponse>> GetByTypeAsync(string type);
    Task EvaluateAlertsAsync();
}
