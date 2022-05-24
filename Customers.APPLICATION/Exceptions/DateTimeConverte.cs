using System.Globalization;
using AutoMapper;

namespace Application.Exceptions;

public class DateTimeTypeConverter : ITypeConverter<string, DateTime>
{
    public DateTime Convert(string source, DateTime destination, ResolutionContext context)
    {
        var result = DateTime.ParseExact(source, "dd-MM-yyyy", null);
        return System.Convert.ToDateTime(result);
    }
}