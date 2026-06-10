namespace UrbansenseAPI.DTOs;

/// <summary>Dados climáticos de uma cidade em um momento específico</summary>
public record WeatherResponse(
    /// <summary>Nome da cidade</summary>
    string City,
    /// <summary>Latitude</summary>
    double Latitude,
    /// <summary>Longitude</summary>
    double Longitude,
    /// <summary>Temperatura em °C</summary>
    double? Temperature,
    /// <summary>Sensação térmica em °C</summary>
    double? FeelsLike,
    /// <summary>Umidade relativa do ar (%)</summary>
    int? Humidity,
    /// <summary>Velocidade do vento em m/s</summary>
    double? WindSpeed,
    /// <summary>Precipitação na última hora em mm</summary>
    double? RainMm,
    /// <summary>Índice UV (0–11+)</summary>
    int? UvIndex,
    /// <summary>Condição climática: Rain, Storm, Clear, Cloudy, Other</summary>
    string Condition,
    /// <summary>Descrição textual do clima</summary>
    string Description,
    /// <summary>Data e hora da medição (UTC)</summary>
    DateTime RecordedAt
);
