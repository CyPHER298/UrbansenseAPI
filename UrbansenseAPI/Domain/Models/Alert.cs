using System.ComponentModel.DataAnnotations;
using UrbansenseAPI.Domain.Enums;

namespace UrbansenseAPI.Domain.Models;

public class Alert
{
    [Key] public long id { get; set; }
    public string city { get; set; }
    public AlertType type { get; set; }
    public Severity severity { get; set; }
    public string message { get; set; }
    public string region { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
    public DateTime validFrom { get; set; }
    public DateTime validUntil { get; set; }
    public bool active { get; set; }

    // FK para TransitLine (nullable)
    public long? transitLineId { get; set; }
    public TransitLine? TransitLine { get; set; }
}
