using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Dto;

public class CreateSurveyDto
{
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2048)]
    public string? Description { get; set; }

    public Guid? FolderId { get; set; }
}
