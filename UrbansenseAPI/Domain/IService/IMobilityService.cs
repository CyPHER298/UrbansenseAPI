using UrbansenseAPI.DTOs;

namespace UrbansenseAPI.Domain.Services;

public interface IMobilityService
{
    Task<List<LineRiskResponse>> GetLinesAtRiskAsync(string city, double lat, double lon);
}
