using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Types;

namespace Domain.Entities.Products;

public class Product : BaseEntityGuid
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(200)]
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public byte ProductCategoryId { get; set; }
    [ForeignKey(nameof(ProductCategoryId))]
    public virtual ProductCategory Category { get; set; }
    public virtual ICollection<ProductImage> Images { get; set; }
}