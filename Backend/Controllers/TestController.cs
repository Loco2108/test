using Backend.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TestController : ControllerBase
{
    private readonly StimmtiDbContext _context;
    private readonly IHubContext<PollHub, IPollClient> _hubContext;

    public TestController(StimmtiDbContext context, IHubContext<PollHub, IPollClient> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Poll>>> GetPolls()
    {
        var polls = await _context.Polls
            .OrderBy(el => el.CreatedAt)
            .ToListAsync();

        return Ok(polls);
    }

    [HttpPost]
    public async Task<ActionResult<Poll>> CreatePoll([FromBody] PollCreateDto poll, [FromHeader(Name = "X-Connection-Id")] string? connectionId)
    {
        var newPoll = new Poll
        {
            Question = poll.Question
        };

        _context.Polls.Add(newPoll);
        await _context.SaveChangesAsync();

        if (!string.IsNullOrEmpty(connectionId))
        {
            await _hubContext.Clients.AllExcept([connectionId]).NewPollCreated(newPoll);
        }
        else
        {
            await _hubContext.Clients.All.NewPollCreated(newPoll);
        }

        return Ok(newPoll);
    }
}