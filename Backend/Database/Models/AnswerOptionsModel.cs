using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class AnswerOption
{
    [Key]
    public Guid Id { get; set; }

    public required int OrderId { get; set; }

    public required string Description { get; set; }

    public required Guid QuestionTemplateId { get; set; }
    public QuestionTemplate? QuestionTemplate { get; set; }

    public Answer? Answer;
}