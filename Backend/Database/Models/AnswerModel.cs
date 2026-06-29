using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public abstract class Answer
{
    [Key]
    public Guid Id { get; set; }

    public Guid AnonymousUserId { get; set; }
    public required AnonymousUser AnonymousUser { get; set; }

    public Guid QuestionId { get; set; }
    public required Question Question { get; set; }
}

public abstract class ChoiceAnswer : Answer
{
    public Guid AnswerOptionId { get; set; }
    public required AnswerOption AnswerOption { get; set; }
}

public class SingleChoiceAnswer : ChoiceAnswer { }

public class MultipleChoiceAnswer : ChoiceAnswer { }

public abstract class TextAnswer : Answer
{
    [MaxLength(2048)]
    public required string Text { get; set; }
}

public class WordCloudAnswer : TextAnswer { }

public class FreeTextAnswer : TextAnswer { }

public class NumberScaleAnswer : Answer
{
    public required int Value { get; set; }
}