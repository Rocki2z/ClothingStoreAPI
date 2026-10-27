namespace ClothingStoreAPI.Models;

public class Order
{
    public int Id { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public decimal TotalAmount { get; set; }

    public ICollection<OrderItem> Items { get; set; }
        = new List<OrderItem>();
}