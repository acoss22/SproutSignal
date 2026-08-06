using System.ComponentModel.DataAnnotations;

namespace SproutSignal.Web.Models;

public class Plant
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(150)]
    public string? Species { get; set; }

    [StringLength(100)]
    public string? Location { get; set; }

    [Range(1, 365)]
    public int WateringIntervalDays { get; set; } = 7;

    public DateTime? LastWateredAtUtc { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }

    [Required]
    public string OwnerId { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
