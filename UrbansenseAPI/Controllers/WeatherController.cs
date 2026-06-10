using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbansenseAPI.Domain.Services;
using UrbansenseAPI.DTOs;

namespace UrbansenseAPI.Controllers;

/// <summary>Endpoints de dados climáticos em tempo real</summary>
[Authorize]
[ApiController]
[Route("api/v1/weather")]
[Produces("application/json")]
public class WeatherController(IWeatherService weatherService) : ControllerBase
{
    /// <summary>Retorna o clima atual de uma cidade</summary>
    /// <param name="city">Nome da cidade (padrão: São Paulo)</param>
    /// <param name="lat">Latitude da cidade</param>
    /// <param name="lon">Longitude da cidade</param>
    /// <returns>Dados climáticos atuais incluindo temperatura, chuva, vento e condição</returns>
    /// <remarks>
    /// Busca dados em tempo real via OpenWeather API e armazena no histórico interno.
    ///
    ///     GET /api/v1/weather/current?city=São Paulo&amp;lat=-23.5505&amp;lon=-46.6333
    ///
    /// </remarks>
    [HttpGet("current")]
    [ProducesResponseType(typeof(WeatherResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> GetCurrent(
        [FromQuery] string city = "São Paulo",
        [FromQuery] double lat  = -23.5505,
        [FromQuery] double lon  = -46.6333)
    {
        var result = await weatherService.GetCurrentWeatherAsync(city, lat, lon);
        return Ok(result);
    }

    /// <summary>Analisa o risco histórico de chuva para um horário específico</summary>
    /// <param name="city">Nome da cidade</param>
    /// <param name="hour">Hora do dia (0–23). Omita para usar a hora atual</param>
    /// <returns>Score de risco, nível (Low/Medium/High) e médias históricas</returns>
    /// <remarks>
    /// Calcula o risco com base nos últimos 30 dias de histórico.
    /// Score &gt;= 0.7 = High, &gt;= 0.4 = Medium, abaixo = Low.
    ///
    ///     GET /api/v1/weather/rain-risk?city=São Paulo&amp;hour=17
    ///
    /// </remarks>
    [HttpGet("rain-risk")]
    [ProducesResponseType(typeof(RainRiskResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetRainRisk(
        [FromQuery] string city = "São Paulo",
        [FromQuery] int    hour = -1)
    {
        var targetHour = hour < 0 ? DateTime.UtcNow.Hour : hour;
        var result = await weatherService.AnalyzeRainRiskAsync(city, targetHour);
        return Ok(result);
    }

    /// <summary>Lista áreas com chuva forte na última hora</summary>
    /// <param name="thresholdMm">Limiar de precipitação em mm/h (padrão: 20mm)</param>
    /// <returns>Lista de cidades com chuva acima do limiar na última hora</returns>
    /// <remarks>
    /// Retorna lista vazia se nenhuma área monitorada atingiu o limiar.
    ///
    ///     GET /api/v1/weather/heavy-rain?thresholdMm=20
    ///
    /// </remarks>
    [HttpGet("heavy-rain")]
    [ProducesResponseType(typeof(IEnumerable<WeatherResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetHeavyRain(
        [FromQuery] double thresholdMm = 20.0)
    {
        var result = await weatherService.GetHeavyRainAreasAsync(thresholdMm);
        return Ok(result);
    }
}
