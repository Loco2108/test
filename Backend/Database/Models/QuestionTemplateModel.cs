using System.ComponentModel.DataAnnotations;
using Backend.Models.Enums;

namespace Backend.Models;

public abstract class QuestionTemplate
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(255)]
    public required string Name { get; set; }

    [MaxLength(2048)]
    public required string Description { get; set; }

    public required Guid SurveyId { get; set; }
    public Survey? Survey { get; set; }

    public required QuestionTypeEnum QuestionType { get; set; }

    public List<Question> Questions { get; } = new List<Question>();
}

public abstract class ChoiceQuestionTemplate : QuestionTemplate
{
    public List<AnswerOption> AnswerOptions { get; } = new List<AnswerOption>();
}

public class SingleChoiceQuestionTemplate : ChoiceQuestionTemplate { }

public class MultipleChoiceQuestionTemplate : ChoiceQuestionTemplate { }

public class WordCloudQuestionTemplate : QuestionTemplate { }

public class FreeTextQuestionTemplate : QuestionTemplate { }

public class NumberScaleQuestionTemplate : QuestionTemplate { }