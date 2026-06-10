namespace UrbansenseAPI.DTOs;

/// <summary>Linha de metrô/CPTM em risco devido à chuva atual</summary>
public record LineRiskResponse(
    /// <summary>Código da linha (ex: L9)</summary>
    string Code,
    /// <summary>Nome completo (ex: Linha 9 - Esmeralda)</summary>
    string Name,
    /// <summary>Operadora (Metrô SP, CPTM, ViaQuatro, ViaMobilidade)</summary>
    string Operator,
    /// <summary>Cor hexadecimal da linha</summary>
    string Color,
    /// <summary>Trecho mais vulnerável a alagamentos</summary>
    string VulnerableSection,
    /// <summary>Nível de vulnerabilidade: Low, Medium, High, Critical</summary>
    string Vulnerability,
    /// <summary>Atraso médio esperado em percentual</summary>
    int AvgDelayPct,
    /// <summary>Estimativa de incidentes históricos em dias de chuva</summary>
    int HistoricalIncidents,
    /// <summary>Mensagem de risco gerada para o passageiro</summary>
    string RiskMessage,
    /// <summary>Precipitação atual em mm/h que ativou o alerta</summary>
    double CurrentRainMm
);
