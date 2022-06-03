using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Products;

public class ProductImage : BaseImageEntity
{
    public string ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; }
}