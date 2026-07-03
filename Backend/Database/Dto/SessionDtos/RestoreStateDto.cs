using Backend.Models.Enums;
using TypeGen.Core.TypeAnnotations;

namespace Backend.Dto;

[ExportTsInterface]
public class RestoreStateDto
{
    public required string SessionName { get; set; }
    public string? SessionDescription { get; set; }

    public required ParticipantRole Role { get; set; }
    public required GameState GameState { get; set; }
    public AnonymousUserDto? UserInformation { get; set; }
}