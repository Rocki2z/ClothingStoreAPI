namespace ClothingStoreAPI.DTOs;

public class OrderResponseDto
{
    public int Id { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public decimal TotalAmount { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}