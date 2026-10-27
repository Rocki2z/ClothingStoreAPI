using System.ComponentModel.DataAnnotations;

namespace ClothingStoreAPI.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }

    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public uint Version { get; set; }
}