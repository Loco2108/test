using AutoMapper;
using Backend.Dto;
using Backend.Models;
using Backend.Models.Enums;
using Backend.StaticHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Hubs;

public class DefaultHub : Hub
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
            .Include(x => x.Survey)
            .FirstOrDefaultAsync(x => x.RoomCode == data.RoomCode);

        if (session == null || session.RoomActive == false) return null;

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
                GameState = session.CurrentState
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
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, data.RoomCode);

        return new RestoreStateDto
        {
            SessionName = session.Name,
            SessionDescription = session.Description,
            Role = ParticipantRole.Participant,
            GameState = session.CurrentState,
            UserInformation = _mapper.Map<AnonymousUserDto>(participant),
        };
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
    }
}