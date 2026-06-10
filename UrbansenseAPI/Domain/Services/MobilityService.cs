using UrbansenseAPI.Domain.Models;
using UrbansenseAPI.DTOs;

namespace UrbansenseAPI.Domain.Services;

public class MobilityService(IWeatherService weatherService) : IMobilityService
{
    // Linhas do Metrô/CPTM de São Paulo com dados de vulnerabilidade histórica
    private static readonly List<TransitLine> _lines =
    [
        new() { id=1,  code="L1", name="Linha 1 - Azul",       technician="Metrô SP",  color="#0155A8", rainVulnerability=0.7, rainThresholdMm=15, vulnerableSection="Tucuruvi - Jabaquara",       avgDelayPctOnRain=25 },
        new() { id=2,  code="L2", name="Linha 2 - Verde",      technician="Metrô SP",  color="#007E5E", rainVulnerability=0.5, rainThresholdMm=20, vulnerableSection="Vila Madalena - Vila Prudente",avgDelayPctOnRain=15 },
        new() { id=3,  code="L3", name="Linha 3 - Vermelha",   technician="Metrô SP",  color="#EE3124", rainVulnerability=0.8, rainThresholdMm=10, vulnerableSection="Itaquera - Barra Funda",avgDelayPctOnRain=30 },
        new() { id=4,  code="L4", name="Linha 4 - Amarela",    technician="ViaQuatro", color="#FFD400", rainVulnerability=0.3, rainThresholdMm=25, vulnerableSection="Luz - Butantã",             avgDelayPctOnRain=10 },
        new() { id=5,  code="L5", name="Linha 5 - Lilás",      technician="ViaMobilidade",color="#9B2990",rainVulnerability=0.5,rainThresholdMm=20,vulnerableSection="Capão Redondo - Chácara Klabin",avgDelayPctOnRain=15 },
        new() { id=6,  code="L7", name="Linha 7 - Rubi",       technician="CPTM",      color="#DC241F", rainVulnerability=0.9, rainThresholdMm=8,  vulnerableSection="Luz - Francisco Morato",    avgDelayPctOnRain=40 },
        new() { id=7,  code="L8", name="Linha 8 - Diamante",   technician="ViaMobilidade",color="#97999B",rainVulnerability=0.85,rainThresholdMm=10,vulnerableSection="Júlio Prestes - Amador Bueno",avgDelayPctOnRain=35 },
        new() { id=8,  code="L9", name="Linha 9 - Esmeralda",  technician="ViaMobilidade",color="#01A651",rainVulnerability=0.95,rainThresholdMm=5, vulnerableSection="Osasco - Grajaú",           avgDelayPctOnRain=50 },
        new() { id=9,  code="L10",name="Linha 10 - Turquesa",  technician="CPTM",      color="#009FC4", rainVulnerability=0.8, rainThresholdMm=12, vulnerableSection="Mauá - Brás",               avgDelayPctOnRain=30 },
        new() { id=10, code="L11",name="Linha 11 - Coral",     technician="CPTM",      color="#F26522", rainVulnerability=0.6, rainThresholdMm=18, vulnerableSection="Luz - Guaianases",          avgDelayPctOnRain=20 },
        new() { id=11, code="L12",name="Linha 12 - Safira",    technician="CPTM",      color="#133E8E", rainVulnerability=0.55,rainThresholdMm=20, vulnerableSection="Brás - Calmon Viana",       avgDelayPctOnRain=18 },
        new() { id=12, code="L13",name="Linha 13 - Jade",      technician="CPTM",      color="#00B398", rainVulnerability=0.3, rainThresholdMm=30, vulnerableSection="Engenheiro Goulart - Guarulhos", avgDelayPctOnRain=10 }
    ];

    public async Task<List<LineRiskResponse>> GetLinesAtRiskAsync(string city, double lat, double lon)
    {
        var weather = await weatherService.GetCurrentWeatherAsync(city, lat, lon);
        var currentRain = weather.RainMm ?? 0.0;

        var linesAtRisk = _lines
            .Where(l => currentRain >= l.rainThresholdMm)
            .OrderByDescending(l => l.rainVulnerability)
            .Select(l => BuildLineRisk(l, currentRain))
            .ToList();

        return linesAtRisk;
    }

    public static IReadOnlyList<TransitLine> GetAllLines() => _lines.AsReadOnly();

    private static LineRiskResponse BuildLineRisk(TransitLine line, double currentRainMm)
    {
        var vulnerability = line.rainVulnerability switch
        {
            >= 0.8 => "Critical",
            >= 0.6 => "High",
            >= 0.4 => "Medium",
            _      => "Low"
        };

        var message = $"{line.name}: chuva atual {currentRainMm:F1}mm excede limiar de " +
                      $"{line.rainThresholdMm}mm. Trecho vulnerável: {line.vulnerableSection}. " +
                      $"Atraso médio esperado: {line.avgDelayPctOnRain}%.";

        return new LineRiskResponse(
            Code: line.code,
            Name: line.name,
            Operator: line.technician,
            Color: line.color,
            VulnerableSection: line.vulnerableSection,
            Vulnerability: vulnerability,
            AvgDelayPct: line.avgDelayPctOnRain,
            HistoricalIncidents: EstimateIncidents(line.rainVulnerability),
            RiskMessage: message,
            CurrentRainMm: currentRainMm
        );
    }

    private static int EstimateIncidents(double vulnerability) =>
        (int)(vulnerability * 45);
}
