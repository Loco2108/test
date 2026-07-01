using Backend.Models.Enums;

namespace Backend.Dto;

public class AddSurveyQuestionDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public QuestionTypeEnum QuestionTypeId { get; set; }
    public List<AddSurveyAnswerOptionDto> AnswerOptions { get; set; } = new();
}

public class AddSurveyAnswerOptionDto
{
    public int OrderId { get; set; }
    public string Description { get; set; } = string.Empty;
}
