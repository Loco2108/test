using AutoMapper;
using Backend.Dto;
using Backend.Models;

namespace Backend.Mapper;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<AnonymousUser, AnonymousUserDto>();
        CreateMap<Session, CreateSessionResponseDto>();
        CreateMap<AnonymousUserDto, AnonymousUser>();
        CreateMap<User, PresenterDto>();
        CreateMap<AnonymousUser, ParticipantDto>();
        CreateMap<AnonymousProfilePicture, AnonymousProfilePictureDto>();
        CreateMap<AnonymousProfilePictureDto, AnonymousProfilePicture>();
        CreateMap<QuestionTemplate, QuestionTemplateDto>();
    }
}