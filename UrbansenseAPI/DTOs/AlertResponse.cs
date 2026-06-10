namespace UrbansenseAPI.DTOs;

/// <summary>Alerta urbano gerado pelas regras de negócio</summary>
public record AlertResponse(
    /// <summary>Identificador do alerta</summary>
    long? Id,
    /// <summary>Cidade afetada</summary>
    string City,
    /// <summary>Tipo: Flooding, HeavyRain, HighUv, TransitDelay, TrafficImpact, HeatWave, StrongWind</summary>
    string Type,
    /// <summary>Severidade: Low, Medium, High, Critical</summary>
    string Severity,
    /// <summary>Mensagem descritiva do alerta</summary>
    string Message,
    /// <summary>Região específica afetada (opcional)</summary>
    string? Region,
    /// <summary>Latitude do ponto de origem</summary>
    double? Lat,
    /// <summary>Longitude do ponto de origem</summary>
    double? Lon,
    /// <summary>Início da validade do alerta (UTC)</summary>
    DateTime ValidFrom,
    /// <summary>Fim da validade do alerta (UTC)</summary>
    DateTime ValidUntil,
    /// <summary>Indica se o alerta ainda está ativo</summary>
    bool Active
);
