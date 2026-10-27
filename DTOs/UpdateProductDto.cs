using System.ComponentModel.DataAnnotations;

namespace ClothingStoreAPI.DTOs;

public class UpdateProductDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Range(typeof(decimal), "0.01", "1000000")]
    public decimal Price { get; set; }

    [Range(0, 100000)]
    public int Stock { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Category { get; set; } = string.Empty;
}