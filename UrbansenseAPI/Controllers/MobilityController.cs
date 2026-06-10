using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbansenseAPI.Domain.Services;
using UrbansenseAPI.DTOs;

namespace UrbansenseAPI.Controllers;

/// <summary>Endpoints de mobilidade urbana e impacto climático nas linhas de metrô/CPTM</summary>
[Authorize]
[ApiController]
[Route("api/v1/mobility")]
[Produces("application/json")]
public class MobilityController(IMobilityService mobilityService) : ControllerBase
{
    /// <summary>Retorna linhas de metrô/CPTM em risco com base na chuva atual</summary>
    /// <param name="city">Nome da cidade (padrão: São Paulo)</param>
    /// <param name="lat">Latitude da cidade</param>
    /// <param name="lon">Longitude da cidade</param>
    /// <returns>Lista de linhas cujo limiar de chuva foi atingido, ordenadas por vulnerabilidade</returns>
    /// <remarks>
    /// Consulta o clima atual e cruza com os limiares históricos de cada linha.
    /// Retorna lista vazia se a chuva atual não atingir o limiar de nenhuma linha.
    ///
    /// Linhas monitoradas: L1-Azul, L2-Verde, L3-Vermelha, L4-Amarela, L5-Lilás,
    /// L7-Rubi, L8-Diamante, L9-Esmeralda, L10-Turquesa, L11-Coral, L12-Safira, L13-Jade.
    ///
    ///     GET /api/v1/mobility/lines-at-risk?city=São Paulo
    ///
    /// </remarks>
    [HttpGet("lines-at-risk")]
    [ProducesResponseType(typeof(IEnumerable<LineRiskResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> GetLinesAtRisk(
        [FromQuery] string city = "São Paulo",
        [FromQuery] double lat  = -23.5505,
        [FromQuery] double lon  = -46.6333)
    {
        var result = await mobilityService.GetLinesAtRiskAsync(city, lat, lon);
        return Ok(result);
    }
}
