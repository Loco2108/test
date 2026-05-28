using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class AnswerOption
{
    [Key]
    public Guid Id { get; set; }

    public required int OrderId { get; set; }

    public required string Description { get; set; }

    public Guid QuestionTemplateId { get; set; }
    public required QuestionTemplate QuestionTemplate { get; set; }

    public Answer? Answer;
}