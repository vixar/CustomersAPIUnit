using Domain.Entities;
using Domain.Entities.Products;
using Domain.Entities.Types;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Application.DTOs.Product;

public class ProductDto : BaseEntity
{
    public string? ProductId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public byte? ProductCategoryId { get; set; }
    public ProductCategoryDto? Category { get; set; }
    public List<ProductImageDto>? Images { get; set; }
}

public class ProductImageDto : BaseEntity
{
    public string? ProductImageId { get; set; }
    public string? Large { get; set; }
    public string? Medium { get; set; }
    public string? Small { get; set; }
    public string? Main { get; set; }
    public string? Description { get; set; }
    public string? ProductId { get; set; }
}

public class ProductCategoryDto : BaseEntity
{
    public byte ProductCategoryId { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
}