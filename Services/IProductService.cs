using ClothingStoreAPI.DTOs;

namespace ClothingStoreAPI.Services;

public interface IProductService
{
    Task<IReadOnlyList<ProductResponseDto>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<ProductResponseDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<ProductResponseDto> CreateAsync(
        CreateProductDto dto,
        CancellationToken cancellationToken = default);

    Task<ProductResponseDto?> UpdateAsync(
        int id,
        UpdateProductDto dto,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<PagedResponseDto<ProductResponseDto>> GetPagedAsync(
    ProductQueryDto query,
    CancellationToken cancellationToken = default);
}