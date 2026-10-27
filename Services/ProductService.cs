using ClothingStoreAPI.Data;
using ClothingStoreAPI.DTOs;
using ClothingStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClothingStoreAPI.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProductResponseDto>>
        GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Products
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                Category = p.Category,
                CreatedAtUtc = p.CreatedAtUtc
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductResponseDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _db.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                Category = p.Category,
                CreatedAtUtc = p.CreatedAtUtc
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ProductResponseDto> CreateAsync(
        CreateProductDto dto,
        CancellationToken cancellationToken = default)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            Category = dto.Category,
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.Products.Add(product);

        await _db.SaveChangesAsync(cancellationToken);

        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            Category = product.Category,
            CreatedAtUtc = product.CreatedAtUtc
        };
    }

    public async Task<ProductResponseDto?> UpdateAsync(
        int id,
        UpdateProductDto dto,
        CancellationToken cancellationToken = default)
    {
        var product = await _db.Products
            .FirstOrDefaultAsync(
                p => p.Id == id,
                cancellationToken);

        if (product == null)
        {
            return null;
        }

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.Category = dto.Category;

        await _db.SaveChangesAsync(cancellationToken);

        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            Category = product.Category,
            CreatedAtUtc = product.CreatedAtUtc
        };
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var product = await _db.Products
            .FirstOrDefaultAsync(
                p => p.Id == id,
                cancellationToken);

        if (product == null)
        {
            return false;
        }

        _db.Products.Remove(product);

        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<PagedResponseDto<ProductResponseDto>>
    GetPagedAsync(
        ProductQueryDto query,
        CancellationToken cancellationToken = default)
{
    var page = Math.Max(1, query.Page);
    var pageSize = Math.Clamp(query.PageSize, 1, 100);

    var products = _db.Products
        .AsNoTracking()
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(query.Search))
    {
        var search = query.Search.Trim();

        products = products.Where(p =>
            p.Name.Contains(search) ||
            p.Category.Contains(search));
    }

    var orderedProducts = query.SortBy.ToLowerInvariant() switch
    {
        "price" => query.Descending
            ? products.OrderByDescending(p => p.Price)
            : products.OrderBy(p => p.Price),

        "stock" => query.Descending
            ? products.OrderByDescending(p => p.Stock)
            : products.OrderBy(p => p.Stock),

        _ => query.Descending
            ? products.OrderByDescending(p => p.Name)
            : products.OrderBy(p => p.Name)
    };

    var totalCount = await products.CountAsync(cancellationToken);

    var items = await orderedProducts
        .ThenBy(p => p.Id)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            Category = p.Category,
            CreatedAtUtc = p.CreatedAtUtc
        })
        .ToListAsync(cancellationToken);

    return new PagedResponseDto<ProductResponseDto>
    {
        Items = items,
        Page = page,
        PageSize = pageSize,
        TotalCount = totalCount
    };

}
}