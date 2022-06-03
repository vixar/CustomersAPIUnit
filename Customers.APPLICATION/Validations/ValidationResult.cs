using System.Runtime.CompilerServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Application.Validations;

public class ValidationResult
{
    public string Code { get; set; }
    public string PropertyName { get; set; }
    public string Message { get; set; }

    public ValidationResult()
    {
        
    }

    public ValidationResult(string code, string propertyName, string message)
    {
        Code = code;
        PropertyName = propertyName;
        Message = message;
    }
    

}

public static class ServiceValidator
{

    public static async Task<Tuple<IEnumerable<ValidationResult>?, bool>> ServiceValidationAsync<T>(T entity, IValidator<T> validator)
    {
        var validation = await validator.ValidateAsync(entity);
        
        return validation.IsValid 
            ? Tuple.Create<IEnumerable<ValidationResult>?, bool>(null, true) 
            : Tuple.Create(validation.Errors?.Select(e => new ValidationResult(
                e.ErrorCode,
                e.PropertyName,
                e.ErrorMessage
            )), false);
    }
}