using ClothingStoreAPI.DTOs;
using ClothingStoreAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service; // Dependency injection of the product service

    public ProductsController(IProductService service) // Constructor injection of the product service
    {
        _service = service;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductResponseDto>>>
        GetAll(CancellationToken cancellationToken)
    {
        var products = await _service.GetAllAsync(cancellationToken);

        return Ok(products);
    }

    // GET: api/products/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var product = await _service.GetByIdAsync(
            id, cancellationToken);

        if (product == null)
        {
            return NotFound(new
            {
                message = "Product not found."
            });
        }

        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create(
        CreateProductDto dto,
        CancellationToken cancellationToken)
    {
        var product = await _service.CreateAsync(
            dto, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    // PUT: api/products/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductResponseDto>> Update(
        int id,
        UpdateProductDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var product = await _service.UpdateAsync(
                id, dto, cancellationToken);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found."
                });
            }

            return Ok(product);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new
            {
                message = "This product was updated by another user. Please reload and try again."
            });
        }
    }

    // DELETE: api/products/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var deleted = await _service.DeleteAsync(
            id, cancellationToken);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Product not found."
            });
        }

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<PagedResponseDto<ProductResponseDto>>>
    Search(
        [FromQuery] ProductQueryDto query,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetPagedAsync(
            query, cancellationToken);

        return Ok(result);
    }


}