using Backend.Dto;
using Backend.Hubs;
using Backend.Mapper;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/v1/[controller]")]
public class SessionController : ControllerBase
{
    private readonly StimmtiDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UserController> _logger;
    private readonly IApiMapper _mapper;
    private readonly IHubContext<DefaultHub> _hubContext;

    public SessionController(StimmtiDbContext context, UserManager<User> userManager, ILogger<UserController> logger, IApiMapper mapper, IHubContext<DefaultHub> hubContext)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _hubContext = hubContext;
    }

    [HttpPost("createSession")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(typeof(CreateSessionResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateSession(CreateSessionDto data)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var survey = await _context.Surveys
            .Include(x => x.QuestionTemplates)
            .FirstOrDefaultAsync(x => x.Id == data.SurveyId);

        if (survey == null)
            return BadRequest(new ProblemDetails { Title = "Survey not found", Detail = $"Survey with ID {data.SurveyId} doesn't exist" });

        if (survey.QuestionTemplates.Count == 0)
            return BadRequest(new ProblemDetails { Title = "Survey has no questions", Detail = $"Survey with ID {data.SurveyId} has no questions" });

        if (survey.OwnerId != user.Id)
            return Forbid();

        var possibleCharacters = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // Removed: O/0, I/1
        var roomCode = "";

        var rng = new Random();

        var maxRetries = 3;
        for (int i = 0; i < maxRetries; i++)
        {
            roomCode = "";
            for (int j = 0; j < 6; j++)
            {
                roomCode += possibleCharacters[rng.Next(possibleCharacters.Length)];
                if (j == 2) roomCode += "-";
            }

            var existingSession = await _context.Sessions.FirstOrDefaultAsync(x => x.RoomCode == roomCode && x.RoomActive == true);
            if (existingSession == null) break;
            if (i == maxRetries - 1)
            {
                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    new ProblemDetails
                    {
                        Title = "No room code available",
                        Detail = "Could not generate a unique room code. Please try again."
                    }
                );
            }
        }

        var session = new Session
        {
            SurveyId = survey.Id,
            Name = data.Name,
            Description = data.Description,
            RoomCode = roomCode,
        };

        foreach (var template in survey.QuestionTemplates)
        {
            var question = new Question
            {
                SessionId = session.Id,
                QuestionTemplateId = template.Id,
            };
            session.Questions.Add(question);
        }

        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();

        return Ok(_mapper.MapToCreateSessionResponseDto(session));
    }

    [HttpGet("checkSession")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckSession([FromQuery] string roomCode)
    {
        var session = await _context.Sessions.FirstOrDefaultAsync(x => x.RoomCode == roomCode.Trim());
        if (session == null || session.RoomActive == false) return NotFound(new ProblemDetails
        {
            Title = "Room not available",
            Detail = $"No active room with code {roomCode} could be found"
        });

        return Ok();
    }
}