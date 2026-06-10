using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbansenseAPI.Domain.Services;
using UrbansenseAPI.DTOs;

namespace UrbansenseAPI.Controllers;

/// <summary>Endpoints de alertas urbanos por regras de negócio</summary>
[Authorize]
[ApiController]
[Route("api/v1/alerts")]
[Produces("application/json")]
public class AlertController(IAlertService alertService) : ControllerBase
{
    /// <summary>Retorna todos os alertas ativos de uma cidade</summary>
    /// <param name="city">Nome da cidade (ex: São Paulo, Guarulhos, Osasco)</param>
    /// <returns>Lista de alertas ordenados por severidade (Critical → Low)</returns>
    /// <remarks>
    /// Alertas expirados são filtrados automaticamente.
    ///
    ///     GET /api/v1/alerts/city/São Paulo
    ///
    /// </remarks>
    [HttpGet("city/{city}")]
    [ProducesResponseType(typeof(IEnumerable<AlertResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByCity(string city)
    {
        var result = await alertService.GetActiveByCityAsync(city);
        return Ok(result);
    }

    /// <summary>Retorna alertas ativos filtrados por tipo</summary>
    /// <param name="type">Tipo do alerta: Flooding, HeavyRain, HighUv, TransitDelay, TrafficImpact, HeatWave, StrongWind</param>
    /// <returns>Lista de alertas do tipo informado</returns>
    /// <remarks>
    /// O parâmetro é case-insensitive.
    ///
    ///     GET /api/v1/alerts/type/Flooding
    ///     GET /api/v1/alerts/type/HighUv
    ///
    /// </remarks>
    [HttpGet("type/{type}")]
    [ProducesResponseType(typeof(IEnumerable<AlertResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetByType(string type)
    {
        var result = await alertService.GetByTypeAsync(type);
        return Ok(result);
    }

    /// <summary>Dispara avaliação das regras de negócio para todas as cidades monitoradas</summary>
    /// <returns>Confirmação de execução</returns>
    /// <remarks>
    /// **Requer role ADMIN.**
    ///
    /// Regras aplicadas:
    /// - Chuva ≥ 40mm/h → Flooding **Critical**
    /// - Chuva ≥ 20mm/h → Flooding **High**
    /// - Chuva ≥ 10mm/h → HeavyRain **Medium**
    /// - UV ≥ 11 → HighUv **High**
    /// - UV ≥ 8 → HighUv **Medium**
    /// - Condição Storm → HeavyRain **Critical**
    ///
    ///     POST /api/v1/alerts/evaluate
    ///
    /// </remarks>
    [Authorize(Roles = "ADMIN")]
    [HttpPost("evaluate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Evaluate()
    {
        await alertService.EvaluateAlertsAsync();
        return Ok(new { message = "Avaliação concluída." });
    }
}
