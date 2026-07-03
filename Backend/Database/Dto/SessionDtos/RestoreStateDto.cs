using Backend.Models.Enums;
using Tapper;

namespace Backend.Dto;

[TranspilationSource]
public class RestoreStateDto
{
    public required string SessionName { get; set; }
    public string? SessionDescription { get; set; }

    public required ParticipantRole Role { get; set; }
    public required GameState GameState { get; set; }
    public AnonymousUserDto? UserInformation { get; set; }
    public required PresenterDto Presenter { get; set; }
    public List<ParticipantDto> Participants { get; set; } = new List<ParticipantDto>();
}