using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Question
{
    [Key]
    public Guid Id { get; set; }

    public Guid SessionId { get; set; }
    public required Session Session { get; set; }

    public Guid QuestionTemplateId { get; set; }
    public required QuestionTemplate QuestionTemplate { get; set; }

    public List<Answer> Answers { get; set; } = new List<Answer>();
}