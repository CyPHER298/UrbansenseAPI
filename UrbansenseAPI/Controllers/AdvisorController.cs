using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbansenseAPI.Domain.Services;
using UrbansenseAPI.DTOs;

namespace UrbansenseAPI.Controllers;

/// <summary>Endpoints do assistente de inteligência urbana com IA</summary>
[Authorize]
[ApiController]
[Route("api/v1/advisor")]
[Produces("application/json")]
public class AdvisorController(IAdvisorService advisorService) : ControllerBase
{
    /// <summary>Faz uma pergunta ao assistente urbano com contexto climático em tempo real</summary>
    /// <param name="request">Objeto com a pergunta e a cidade de referência</param>
    /// <returns>Resposta gerada pela IA com base no contexto climático atual</returns>
    /// <remarks>
    /// O assistente usa RAG (Retrieval-Augmented Generation) com base de conhecimento
    /// sobre histórico urbano de São Paulo e consulta os dados climáticos em tempo real.
    ///
    ///     POST /api/v1/advisor/ask
    ///     {
    ///         "question": "Devo levar guarda-chuva hoje? A Linha 9 vai atrasar?",
    ///         "city": "São Paulo"
    ///     }
    ///
    /// </remarks>
    [HttpPost("ask")]
    [ProducesResponseType(typeof(AdvisorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Ask([FromBody] AskRequest request)
    {
        var result = await advisorService.AskAsync(request);
        return Ok(result);
    }

    /// <summary>Gera um resumo diário completo da cidade</summary>
    /// <param name="city">Nome da cidade (ex: São Paulo, Campinas, Guarulhos)</param>
    /// <returns>Resumo com condições climáticas, alertas ativos e impacto na mobilidade</returns>
    /// <remarks>
    ///     GET /api/v1/advisor/daily-summary/São Paulo
    /// </remarks>
    [HttpGet("daily-summary/{city}")]
    [ProducesResponseType(typeof(AdvisorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetDailySummary(string city)
    {
        var result = await advisorService.GetDailySummaryAsync(city);
        return Ok(result);
    }

    /// <summary>Analisa o impacto climático atual em uma linha específica de metrô/CPTM</summary>
    /// <param name="lineName">Nome ou código da linha (ex: Linha 9, Esmeralda, L9)</param>
    /// <returns>Análise de risco e recomendações para passageiros</returns>
    /// <remarks>
    ///     GET /api/v1/advisor/transit/Linha 9
    ///     GET /api/v1/advisor/transit/Esmeralda
    /// </remarks>
    [HttpGet("transit/{lineName}")]
    [ProducesResponseType(typeof(AdvisorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AnalyzeTransitLine(string lineName)
    {
        var result = await advisorService.AnalyzeTransitLineAsync(lineName);
        return Ok(result);
    }
}
