using Backend.Dto;
using Backend.Models;
using Riok.Mapperly.Abstractions;

// RMG020: Intentional partial mappings – source models contain more fields than the DTOs
#pragma warning disable RMG020

namespace Backend.Mapper;

[Mapper]
public partial class ApiMapper : IApiMapper
{
    public partial AnonymousUserDto MapToAnonymousUserDto(AnonymousUser source);
    public partial CreateSessionResponseDto MapToCreateSessionResponseDto(Session source);
    public partial PresenterDto MapToPresenterDto(User source);
    public partial ParticipantDto MapToParticipantDto(AnonymousUser source);
    public partial AnonymousProfilePictureDto MapToAnonymousProfilePictureDto(AnonymousProfilePicture source);

    [MapperIgnoreTarget(nameof(AnonymousProfilePicture.Id))]
    [MapperIgnoreTarget(nameof(AnonymousProfilePicture.AnonymousUserId))]
    [MapperIgnoreTarget(nameof(AnonymousProfilePicture.AnonymousUser))]
    public partial void UpdateAnonymousProfilePicture(AnonymousProfilePictureDto source, AnonymousProfilePicture target);

    [MapperIgnoreTarget(nameof(QuestionTemplateDto.AnswerOptions))]
    public partial QuestionTemplateDto MapToQuestionTemplateDto(QuestionTemplate source);

    public partial List<ParticipantDto> MapToParticipantDtoList(IEnumerable<AnonymousUser> source);
}