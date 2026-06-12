using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Folder
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    public Guid OwnerId { get; set; }
    public required User Owner { get; set; }

    public List<Survey> Surveys { get; } = new List<Survey>();
}