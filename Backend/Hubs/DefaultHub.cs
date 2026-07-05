using System.Runtime.CompilerServices;
using AutoMapper;
using Backend.Dto;
using Backend.Hubs.Interfaces;
using Backend.Models;
using Backend.Models.Enums;
using Backend.StaticHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Hubs;

public class DefaultHub : Hub<ISessionHubClient>, ISessionHub
{
    private readonly UserManager<User> _userManager;
    private readonly StimmtiDbContext _context;
    private readonly ILogger<DefaultHub> _logger;
    private readonly IMapper _mapper;

    public DefaultHub(UserManager<User> userManager, StimmtiDbContext context, ILogger<DefaultHub> logger, IMapper mapper)
    {
        _userManager = userManager;
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<RestoreStateDto?> JoinSession(JoinSessionDto data)
    {
        var session = await _context.Sessions
            .Include(x => x.AnonymousParticipants)
            .ThenInclude(x => x.ProfilePicture)
            .Include(x => x.Survey)
            .ThenInclude(x => x!.Owner)
            .FirstOrDefaultAsync(x => x.RoomCode == data.RoomCode && x.RoomActive == true);

        if (session == null) return null;

        var currentUserId = Context.UserIdentifier;
        bool isPresenter = currentUserId != null && session.Survey!.OwnerId.ToString() == currentUserId;

        if (isPresenter)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, data.RoomCode);

            return new RestoreStateDto
            {
                SessionName = session.Name,
                SessionDescription = session.Description,
                Role = ParticipantRole.Presenter,
                SessionState = session.CurrentState,
                Presenter = _mapper.Map<PresenterDto>(session.Survey!.Owner),
                Participants = _mapper.Map<List<ParticipantDto>>(session.AnonymousParticipants)
            };
        }

        // --- Anonymous participant
        var participant = session.AnonymousParticipants.FirstOrDefault(x => x.Id == data.playerId);
        if (participant == null)
        {
            participant = new AnonymousUser
            {
                Name = NameGenerator.GenerateName("", " "),
                SessionId = session.Id,
            };

            await _context.AddAsync(participant);

            var rng = new Random();
            T RandomEnum<T>() where T : struct, Enum
            {
                var values = Enum.GetValues<T>();
                return values[rng.Next(values.Length)];
            }

            var anonProfilePicture = new AnonymousProfilePicture
            {
                Body = RandomEnum<BodyProfileEnum>(),
                Color = RandomEnum<ColorProfileEnum>(),
                Face = RandomEnum<FaceProfileEnum>(),
                Hat = RandomEnum<HatProfileEnum>(),
                AnonymousUserId = participant.Id
            };

            await _context.AddAsync(anonProfilePicture);
            await _context.SaveChangesAsync();

            await Clients.Group(data.RoomCode).ParticipantJoined(new ParticipantDto
            {
                Name = participant.Name,
                ProfilePicture = _mapper.Map<AnonymousProfilePictureDto>(anonProfilePicture)
            });
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, data.RoomCode);

        return new RestoreStateDto
        {
            SessionName = session.Name,
            SessionDescription = session.Description,
            Role = ParticipantRole.Participant,
            SessionState = session.CurrentState,
            UserInformation = _mapper.Map<AnonymousUserDto>(participant),
            Presenter = _mapper.Map<PresenterDto>(session.Survey!.Owner),
            Participants = _mapper.Map<List<ParticipantDto>>(session.AnonymousParticipants)
        };
    }

    public async Task LeaveRoom(string roomCode)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomCode);
    }

    public async Task<bool> UpdateParticipantData(ParticipantUpdateDto data)
    {
        var participant = await _context.AnonymousUsers
            .Include(x => x.ProfilePicture)
            .Include(x => x.Session)
            .FirstOrDefaultAsync(x => x.Id == data.AnonymousUserId);

        if (participant == null) return false;
        if (participant.Session!.RoomCode != data.RoomCode) return false;
        if (participant.Session!.RoomActive == false) return false;
        if (!string.IsNullOrWhiteSpace(data.Name) && data.Name.Length > 64) return false;

        var oldName = participant.Name;

        if (!string.IsNullOrWhiteSpace(data.Name))
        {
            var nameAlreadyExists = await _context.AnonymousUsers
                .AnyAsync(p =>
                    p.Session!.RoomCode == data.RoomCode &&
                    p.Name.ToLower() == data.Name.Trim().ToLower() &&
                    p.Id != data.AnonymousUserId
                );

            if (nameAlreadyExists) return false;

            participant.Name = data.Name.Trim();
        }

        if (data.ProfilePicture != null)
        {
            _mapper.Map(data.ProfilePicture, participant.ProfilePicture);
        }

        await _context.SaveChangesAsync();

        await Clients.Group(data.RoomCode).ParticipantUpdated(new ParticipantUpdateResponseDto
        {
            OldName = oldName,
            NewName = participant.Name,
            ProfilePicture = _mapper.Map<AnonymousProfilePictureDto>(participant.ProfilePicture)
        });

        return true;
    }

    public async Task<bool> StartSession(string roomCode)
    {
        var session = await _context.Sessions
            .Include(x => x.Questions.OrderBy(y => y.QuestionTemplate!.OrderNumber))
            .ThenInclude(x => x.QuestionTemplate)
            .Include(x => x.Questions)
            .ThenInclude(x => (x.QuestionTemplate as ChoiceQuestionTemplate)!.AnswerOptions.OrderBy(y => y.OrderNumber))
            .FirstOrDefaultAsync(x => x.RoomCode == roomCode && x.RoomActive == true);
        if (session == null) return false;

        var currentUserId = Context.UserIdentifier;
        if (currentUserId == null || session.Survey!.OwnerId.ToString() != currentUserId) return false;

        if (session.Questions.Count == 0) return false;

        await Clients.Group(roomCode).SessionStateChanged(SessionState.Loading);
        await Clients.Group(roomCode).QuestionChanged(_mapper.Map<QuestionTemplateDto>(session.Questions.First().QuestionTemplate));
        await Clients.Group(roomCode).SessionStateChanged(SessionState.Question);

        return true;
    }

    public Task<bool> NextQuestion(string roomCode)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CloseSession(string roomCode)
    {
        throw new NotImplementedException();
    }
}