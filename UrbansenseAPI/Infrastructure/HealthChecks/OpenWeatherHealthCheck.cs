using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UrbansenseAPI.Infrastructure.HealthChecks;

public class OpenWeatherHealthCheck(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var apiKey  = configuration["OpenWeather:ApiKey"];
            var baseUrl = configuration["OpenWeather:BaseUrl"] ?? "https://api.openweathermap.org";

            var client   = httpClientFactory.CreateClient();
            var url      = $"{baseUrl}/data/2.5/weather?q=São Paulo&appid={apiKey}&units=metric";
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
