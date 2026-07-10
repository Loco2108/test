using Tapper;

namespace Backend.Dto;

[TranspilationSource]
public class AnswerDisplayDto
{
    public List<ChoiceResultDto> ChoiceResults { get; set; } = new();
    public List<FreeTextResultDto> FreeTextResults { get; set; } = new();
    public List<WordCloudResultDto> WordCloudResults { get; set; } = new();
    public List<NumberResultDto> NumberResults { get; set; } = new();

    public int TotalParticipantsAnswered { get; set; }
}

[TranspilationSource]
public class ChoiceResultDto
{
    public required AnswerOptionDto AnswerOption { get; set; }
    public int Count { get; set; }
}

[TranspilationSource]
public class FreeTextResultDto
{
    public required Guid Id { get; set; }
    public required string Text { get; set; }
}

[TranspilationSource]
public class WordCloudResultDto
{
    public required string Text { get; set; }
    public int Count { get; set; }
}

[TranspilationSource]
public class NumberResultDto
{
    public required int Value { get; set; }
    public int Count { get; set; }
}