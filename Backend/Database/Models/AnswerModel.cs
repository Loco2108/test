using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Answer
{
    [Key]
    public Guid Id { get; set; }

    public string? Text { get; set; }

    public Guid? AnswerOptionId { get; set; }
    public AnswerOption? AnswerOption { get; set; }

    public required Guid AnonymousUserId { get; set; }
    public AnonymousUser? AnonymousUser { get; set; }

    public required Guid QuestionId { get; set; }
    public Question? Question { get; set; }
}