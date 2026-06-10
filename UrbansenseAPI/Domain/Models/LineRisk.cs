namespace UrbansenseAPI.Domain.Models;

public class LineRisk
{
    public string code { get; set; }
    public string name { get; set; }
    public string technician { get; set; }
    public string color { get; set; }
    public string vulnerableSection { get; set; }
    public double vulnerability { get; set; }
    public int avgDelayPct { get; set; }
    public int historicalIncidents { get; set; }
    public string riskMessage { get; set; }
    public double currentRainMm { get; set; }
}