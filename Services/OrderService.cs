using ClothingStoreAPI.Data;
using ClothingStoreAPI.DTOs;
using ClothingStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreAPI.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<OrderResponseDto> PlaceOrderAsync(
        CreateOrderDto dto,
        CancellationToken cancellationToken = default)
    {
        if (dto.Quantity <= 0)
        {
            throw new ArgumentException(
                "Quantity must be greater than zero.");
        }

        await using var transaction =
            await _db.Database.BeginTransactionAsync(
                cancellationToken);

        try
        {
            // 1. Find the product and its current price.
            var product = await _db.Products
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    p => p.Id == dto.ProductId,
                    cancellationToken);

            if (product is null)
            {
                throw new KeyNotFoundException(
                    "Product not found.");
            }

            // 2. Reduce stock only if enough stock remains.
            // This conditional update helps prevent overselling.
            var affectedRows = await _db.Products
                .Where(p =>
                    p.Id == dto.ProductId &&
                    p.Stock >= dto.Quantity)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(
                        p => p.Stock,
                        p => p.Stock - dto.Quantity),
                    cancellationToken);

            if (affectedRows == 0)
            {
                throw new InvalidOperationException(
                    "Insufficient stock available.");
            }

            // 3. Calculate the order total on the server.
            var totalAmount = product.Price * dto.Quantity;

            // 4. Create the order and its item.
            var order = new Order
            {
                CreatedAtUtc = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = dto.Quantity,
                        UnitPrice = product.Price
                    }
                }
            };

            _db.Orders.Add(order);

            // 5. Save the order and item.
            // Stock reduction is still uncommitted.
            await _db.SaveChangesAsync(cancellationToken);

            // 6. Commit stock, order and item together.
            await transaction.CommitAsync(cancellationToken);

            return new OrderResponseDto
            {
                Id = order.Id,
                CreatedAtUtc = order.CreatedAtUtc,
                TotalAmount = order.TotalAmount,
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = dto.Quantity,
                UnitPrice = product.Price
            };
        }
        catch
        {
            // If any step fails, undo uncommitted changes.
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<OrderResponseDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _db.Orders
            .AsNoTracking()
            .Where(o => o.Id == id)
            .Select(o => new OrderResponseDto
            {
                Id = o.Id,
                CreatedAtUtc = o.CreatedAtUtc,
                TotalAmount = o.TotalAmount,
                ProductId = o.Items
                    .Select(i => i.ProductId)
                    .FirstOrDefault(),
                ProductName = o.Items
                    .Select(i => i.Product.Name)
                    .FirstOrDefault() ?? string.Empty,
                Quantity = o.Items
                    .Select(i => i.Quantity)
                    .FirstOrDefault(),
                UnitPrice = o.Items
                    .Select(i => i.UnitPrice)
                    .FirstOrDefault()
            })
            .SingleOrDefaultAsync(cancellationToken);
    }
}