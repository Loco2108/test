using Tapper;

namespace Backend.Dto;

[TranspilationSource]
public class PresenterDto
{
    public required string UserName { get; set; }
    public string? ProfilePictureUrl { get; set; }
}