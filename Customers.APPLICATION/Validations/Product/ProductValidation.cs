using Application.DTOs.Product;
using Application.Services.Product.Category;
using Application.Services.Product.Image;
using FluentValidation;

namespace Application.Validations.Product;

public class ProductValidation : AbstractValidator<ProductDto>
{
    public ProductValidation()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Category Id is required");
        
        RuleFor(x => x.Description)
            .NotNull()
            .WithMessage("Category Id is required");
        
        RuleFor(x => x.ProductCategoryId)
            .NotNull()
            .WithMessage("Product Category Id is required");
        
        RuleFor(x => x.ProductCategoryId)
            .GreaterThan((byte)0)
            .WithMessage("Product Category Id is required");
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");
        
        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("Name is required");
        
        RuleFor(x => x.Price)
            .GreaterThan(0.00m)
            .WithMessage("Price is required and must be greater than 0");
        
        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("Quantity is required");
        
        RuleFor(x => x.Quantity)
            .NotNull()
            .WithMessage("Quantity is required");
        
        When(x => (x.Images != null), () =>
        {
            When(x => (x.Images.Count > 0), () =>
            {
                RuleForEach(x => x.Images).ChildRules(x =>
                    x.RuleFor(a => a.Main).NotEmpty().WithMessage("Main Image is required"));
                RuleForEach(x => x.Images).ChildRules(x =>
                    x.RuleFor(a => a.Description).NotEmpty().WithMessage("Main Image is required"));
            });

        });
        

    }
}