using Application.DTOs.Product;
using Application.Services.Product.Category;
using FluentValidation;

namespace Application.Validations.Product;

public class ProductCategoryValidation : AbstractValidator<ProductCategoryDto>
{
    public ProductCategoryValidation()
    {
        RuleFor(x => x.Description).NotEmpty().WithMessage("Category Description is required");
        RuleFor(x => x.Description).NotNull().WithMessage("Category Description is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category Name is required");
    }
}
