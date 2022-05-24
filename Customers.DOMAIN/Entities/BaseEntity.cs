using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class BaseEntity
{
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public DateTime? UpdateAt { get; set; }
    public bool IsActive { get; set; } = true;
    public string Status { get; set; } = "Active";
}

public class BaseEntityTiny: BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public byte Id { get; set; }
}
public class BaseEntityGuid: BaseEntity
{
    [Key, MaxLength(36)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = Guid.NewGuid().ToString();
}

public class TypeBaseEntity<T> : BaseEntityTiny
{
    public string Type { get; set; }
}

