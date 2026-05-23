using System.ComponentModel.DataAnnotations;

public class Poll
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Question { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}