using Backend.Dto;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/v1/[controller]")]
public class SurveyController : ControllerBase
{
	private readonly StimmtiDbContext _dbContext;
	private readonly UserManager<User> _userManager;
	private readonly ILogger<SurveyController> _logger;

	public SurveyController(StimmtiDbContext dbContext, UserManager<User> userManager, ILogger<SurveyController> logger)
	{
		_dbContext = dbContext;
		_userManager = userManager;
		_logger = logger;
	}

	[HttpPost]
	[Authorize]
	[ProducesResponseType(typeof(CreateSurveyResponseDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CreateSurvey([FromBody] CreateSurveyDto data)
	{
		_logger.LogInformation("CreateSurvey called. Authenticated={IsAuthenticated}, UserName={UserName}", User.Identity?.IsAuthenticated, User.Identity?.Name);

		var user = await _userManager.GetUserAsync(User);
		if (user == null) return Unauthorized();

		_logger.LogInformation("CreateSurvey resolved user. UserId={UserId}, Email={Email}", user.Id, user.Email);

		var title = data.Title.Trim();
		if (string.IsNullOrWhiteSpace(title))
		{
			_logger.LogWarning("CreateSurvey validation failed for UserId={UserId}: empty title.", user.Id);
			return BadRequest(new ProblemDetails { Title = "Missing Title", Detail = "No title was provided" });
		}

		if (data.FolderId.HasValue)
		{
			var folder = await _dbContext.Folders.FirstOrDefaultAsync(x => x.Id == data.FolderId.Value);
			if (folder == null)
			{
				_logger.LogWarning("CreateSurvey failed for UserId={UserId}: FolderId={FolderId} not found.", user.Id, data.FolderId.Value);
				return NotFound(new ProblemDetails { Title = "Folder not found", Detail = $"Folder with ID {data.FolderId} doesn't exist" });
			}

			if (folder.OwnerId != user.Id)
			{
				_logger.LogWarning("CreateSurvey forbidden for UserId={UserId}: FolderId={FolderId} owned by {OwnerId}.", user.Id, folder.Id, folder.OwnerId);
				return Forbid();
			}
		}

		var survey = new Survey
		{
			Title = title,
			Description = data.Description,
			FolderId = data.FolderId,
			OwnerId = user.Id,
		};

		_dbContext.Surveys.Add(survey);
		await _dbContext.SaveChangesAsync();

		_logger.LogInformation("CreateSurvey success. SurveyId={SurveyId}, UserId={UserId}, FolderId={FolderId}", survey.Id, user.Id, survey.FolderId);

		return Ok(new CreateSurveyResponseDto
		{
			SurveyId = survey.Id,
			FolderId = data.FolderId
		});
	}

	[HttpPost("folders")]
	[Authorize]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(CreateFolderResponseDto), StatusCodes.Status200OK)]
	public async Task<IActionResult> CreateFolder([FromBody] CreateFolderDto data)
	{
		var user = await _userManager.GetUserAsync(User);
		if (user == null) return Unauthorized();

		var name = data.Name.Trim();
		if (string.IsNullOrWhiteSpace(name))
		{
			_logger.LogWarning("CreateFolder validation failed for UserId={UserId}: empty name.", user.Id);
			return BadRequest(new ProblemDetails { Title = "No name provided", Detail = "No folder name was provided" });
		}

		var folder = new Folder
		{
			Name = name,
			OwnerId = user.Id
		};

		_dbContext.Folders.Add(folder);
		await _dbContext.SaveChangesAsync();

		_logger.LogInformation("CreateFolder success. FolderId={FolderId}, UserId={UserId}, Name={FolderName}", folder.Id, user.Id, folder.Name);

		return Ok(new CreateFolderResponseDto
		{
			FolderId = folder.Id
		});
	}
}
