using Application.DTOs.Product;
using Application.Services.Product.Category;
using Application.Services.Product.Image;
using FluentValidation;

namespace Application.Validations.Product;

public class ProductImageValidation : AbstractValidator<ProductImageDto>
{
    public ProductImageValidation()
    {
        RuleFor(x => x.Description).NotEmpty().WithMessage("Category Id is required");
        RuleFor(x => x.Description).NotNull().WithMessage("Category Id is required");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product Id is required");
    }
}