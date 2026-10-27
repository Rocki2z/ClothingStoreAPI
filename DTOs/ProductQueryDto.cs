namespace ClothingStoreAPI.DTOs;

public class ProductQueryDto
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }

    public string SortBy { get; set; } = "name";

    public bool Descending { get; set; }
}