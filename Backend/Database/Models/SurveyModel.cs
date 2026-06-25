using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Survey
{
    [Key]
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public Guid? FolderId { get; set; }
    public Folder? Folder { get; set; }

    public Guid OwnerId { get; set; }
    public required User Owner { get; set; }

    public List<Session> Sessions { get; } = new List<Session>();
    public List<QuestionTemplate> QuestionTemplates { get; } = new List<QuestionTemplate>();
}