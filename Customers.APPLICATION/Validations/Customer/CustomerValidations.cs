using Application.DTOs.Common;
using Application.Services.Customer;
using FluentValidation;

namespace Application.Validations.Customer;

public class AddCustomerServiceValidations : AbstractValidator<AddCustomerService>
{
    public AddCustomerServiceValidations()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required");
        RuleFor(x => x.FirstName).NotNull().WithMessage("First Name is required");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Email is invalid");
        RuleFor(x => x.Birthday).NotEmpty().WithMessage("Birthday is required");
        RuleFor(x => x.Gender).NotEmpty().WithMessage("Birthday is required");
        RuleFor(x => x.Gender).Length(1).WithMessage("The {PropertyName} has many characters");
        RuleFor(x => (x.ContactNumbers ?? Array.Empty<ContactNumberDto>()).ToList().Count).GreaterThan(0).WithMessage("At least one contact number is required");
        RuleForEach(x => x.ContactNumbers).ChildRules(contactNumbers =>
        {
            contactNumbers.RuleFor(x => x.ContactNumberTypeId).GreaterThan((byte)0)
                .WithMessage("Incorrect contact number type ID");
        });
    }
}

public class UpdateCustomerServiceValidations : AbstractValidator<UpdateCustomerService>
{
    public UpdateCustomerServiceValidations()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required");
        RuleFor(x => x.FirstName).NotNull().WithMessage("First Name is required");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Email is invalid");
        RuleFor(x => x.Birthday).NotEmpty().WithMessage("Birthday is required");
        RuleFor(x => x.Gender).NotEmpty().WithMessage("Birthday is required");
        RuleFor(x => x.Gender).Length(1).WithMessage("The {PropertyName} has many characters");
    }
}
