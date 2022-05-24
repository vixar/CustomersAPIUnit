namespace Application.Validations;

public class ValidationResult
{
    public string Code { get; set; }
    public string PropertyName { get; set; }
    public string Message { get; set; }

    public ValidationResult(string code, string propertyName, string message)
    {
        Code = code;
        PropertyName = propertyName;
        Message = message;
    }
}