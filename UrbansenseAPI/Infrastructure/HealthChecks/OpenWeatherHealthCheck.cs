using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UrbansenseAPI.Infrastructure.HealthChecks;

public class OpenWeatherHealthCheck(IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var apiKey = configuration["OpenWeather:ApiKey"];
            var url    = $"https://api.openweathermap.org/data/2.5/weather?lat=-23.5505&lon=-46.6333&appid={apiKey}&units=metric";

            using var client   = new HttpClient();
            var response = await client.GetAsync(url, cancellationToken);

            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy("OpenWeather API respondendo normalmente.")
                : HealthCheckResult.Degraded($"OpenWeather API retornou {(int)response.StatusCode}.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("OpenWeather API inacessível.", ex);
        }
    }
}
