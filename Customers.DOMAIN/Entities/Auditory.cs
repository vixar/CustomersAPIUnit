using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Auditory
{
    public Auditory(){}

    public int Id { get; set; }
    [MaxLength(36)]
    public string UserId { get; set; }
    public string Type { get; set; }
    public string TableName { get; set; }
    public DateTime DateTime { get; set; }
    public string OldValues { get; set; }
    public string NewValues { get; set; }
    public string AffectedColumns { get; set; }
    public string PrimaryKey { get; set; }
}