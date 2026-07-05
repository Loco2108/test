using Backend.Models.Enums;

namespace Backend.Dto;

public class QuestionTemplateDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required QuestionTypeEnum QuestionType { get; set; }
    public List<AnswerOptionDto> AnswerOptions { get; } = new List<AnswerOptionDto>();
}