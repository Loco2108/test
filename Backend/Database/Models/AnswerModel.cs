using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Answer
{
    [Key]
    public Guid Id { get; set; }

    public string? Text { get; set; }

    public Guid? AnswerOptionId { get; set; }
    public AnswerOption? AnswerOption { get; set; }

    public Guid AnonymousUserId { get; set; }
    public required AnonymousUser AnonymousUser { get; set; }

    public Guid QuestionId { get; set; }
    public required Question Question { get; set; }
}