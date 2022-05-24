using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Person;

public class PersonBaseEntity<T>: BaseEntityGuid
{
    [Required, MaxLength(80)]
    public string FirstName { get; set; }
    [Required, MaxLength(100)]
    public string LastName { get; set; }
    [Required, MaxLength(100)]
    public string Email { get; set; }
    [Required, MaxLength(1)]
    public string Gender { get; set; }
    [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateTime Birthday { get; set; }
}