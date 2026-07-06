using Tapper;

namespace Backend.Dto;

[TranspilationSource]
public class AnswerOptionDto
{
    public required int OrderId { get; set; }
    public required string Description { get; set; }
}