using UrbansenseAPI.DTOs;

namespace UrbansenseAPI.Domain.Services;

public interface IAdvisorService
{
    Task<AdvisorResponse> AskAsync(AskRequest request);
    Task<AdvisorResponse> GetDailySummaryAsync(string city);
    Task<AdvisorResponse> AnalyzeTransitLineAsync(string lineName);
}
