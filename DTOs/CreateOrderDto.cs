using System.ComponentModel.DataAnnotations;

namespace ClothingStoreAPI.DTOs;

public class CreateOrderDto
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, 1000)]
    public int Quantity { get; set; }
}