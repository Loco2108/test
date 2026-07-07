using System.ComponentModel.DataAnnotations;

public class UpdateSurveyDto
{
    [MaxLength(255)]
    public required string? Title { get; set; }

    [MaxLength(2048)]
    public required string? Description { get; set; }

    public Guid? FolderId { get; set; }

    public bool? RemoveFromFolder { get; set; }
}