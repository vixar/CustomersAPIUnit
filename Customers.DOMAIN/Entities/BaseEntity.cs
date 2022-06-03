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

public class TypeBaseEntity : BaseEntityTiny
{
    [MaxLength(250)]
    public string Type { get; set; }
}

public class BaseCategoryEntity : BaseEntityTiny
{
    [MaxLength(50)]
    public string Category { get; set; }
    [MaxLength(250)]
    public string Description { get; set; }
}

public class BaseImageEntity : BaseEntityGuid
{
    [MaxLength(250)]
    public string? Large { get; set; }
    [MaxLength(250)]
    public string? Medium { get; set; }
    [MaxLength(250)]
    public string? Small { get; set; }
    [MaxLength(250)]
    public string Main { get; set; }
    [MaxLength(250)]
    public string Description { get; set; }
    
}

