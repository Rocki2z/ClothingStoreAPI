using ClothingStoreAPI.DTOs;

namespace ClothingStoreAPI.Services;

public interface IOrderService
{
    Task<OrderResponseDto> PlaceOrderAsync(
        CreateOrderDto dto,
        CancellationToken cancellationToken = default);

    Task<OrderResponseDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);
}