using System.ComponentModel.DataAnnotations;

namespace UrbansenseAPI.Domain.Models;

public class TransitLine
{
    [Key] public long id { get; set; }
    public string code { get; set; }
    public string name { get; set; }
    public string technician { get; set; }
    public string color { get; set; }
    public double rainVulnerability { get; set; }
    public double rainThresholdMm { get; set; }
    public string vulnerableSection { get; set; }
    public int avgDelayPctOnRain { get; set; }

    // Relacionamento 1:N
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
}
