using Backend.Dto;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
	public async Task<IActionResult> CreateSurvey([FromBody] CreateSurveyDto? data)
	{
		_logger.LogInformation("CreateSurvey called. Authenticated={IsAuthenticated}, UserName={UserName}", User.Identity?.IsAuthenticated, User.Identity?.Name);

		if (data == null)
		{
			_logger.LogWarning("CreateSurvey validation failed: request body is missing or invalid JSON.");
			return BadRequest("Request body is required.");
		}

		if (!User.Identity?.IsAuthenticated ?? true)
		{
			_logger.LogWarning("CreateSurvey rejected before user lookup: request is not authenticated.");
		}

		var currentUser = await _userManager.GetUserAsync(User);
		if (currentUser == null)
		{
			var subject = User.FindFirst("sub")?.Value;
			var userId = User.FindFirst("nameid")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			_logger.LogWarning("CreateSurvey unauthorized: authenticated principal has no matching app user. sub={Sub}, nameid={NameId}, identityName={IdentityName}", subject, userId, User.Identity?.Name);
			return Unauthorized();
		}

		_logger.LogInformation("CreateSurvey resolved user. UserId={UserId}, Email={Email}", currentUser.Id, currentUser.Email);

		var title = data.Title.Trim();
		if (string.IsNullOrWhiteSpace(title))
		{
			_logger.LogWarning("CreateSurvey validation failed for UserId={UserId}: empty title.", currentUser.Id);
			return BadRequest("Title is required.");
		}

		if (data.FolderId.HasValue)
		{
			var folder = await _dbContext.Folders.FindAsync(data.FolderId.Value);
			if (folder == null)
			{
				_logger.LogWarning("CreateSurvey failed for UserId={UserId}: FolderId={FolderId} not found.", currentUser.Id, data.FolderId.Value);
				return NotFound("Folder not found.");
			}

			if (folder.OwnerId != currentUser.Id)
			{
				_logger.LogWarning("CreateSurvey forbidden for UserId={UserId}: FolderId={FolderId} owned by {OwnerId}.", currentUser.Id, folder.Id, folder.OwnerId);
				return Forbid();
			}
		}

		var survey = new Survey
		{
			Title = title,
			FolderId = data.FolderId,
			OwnerId = currentUser.Id,
			Owner = currentUser
		};

		_dbContext.Surveys.Add(survey);
		await _dbContext.SaveChangesAsync();

		_logger.LogInformation("CreateSurvey success. SurveyId={SurveyId}, UserId={UserId}, FolderId={FolderId}", survey.Id, currentUser.Id, survey.FolderId);

		return Ok(new
		{
			message = "Survey created successfully",
			surveyId = survey.Id
		});
	}

	[HttpPost("folders")]
	[Authorize]
	public async Task<IActionResult> CreateFolder([FromBody] CreateFolderDto? data)
	{
		_logger.LogInformation("CreateFolder called. Authenticated={IsAuthenticated}, UserName={UserName}", User.Identity?.IsAuthenticated, User.Identity?.Name);

		if (data == null)
		{
			_logger.LogWarning("CreateFolder validation failed: request body is missing or invalid JSON.");
			return BadRequest("Request body is required.");
		}

		var currentUser = await _userManager.GetUserAsync(User);
		if (currentUser == null)
		{
			var subject = User.FindFirst("sub")?.Value;
			var userId = User.FindFirst("nameid")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			_logger.LogWarning("CreateFolder unauthorized: authenticated principal has no matching app user. sub={Sub}, nameid={NameId}, identityName={IdentityName}", subject, userId, User.Identity?.Name);
			return Unauthorized();
		}

		var name = data.Name.Trim();
		if (string.IsNullOrWhiteSpace(name))
		{
			_logger.LogWarning("CreateFolder validation failed for UserId={UserId}: empty name.", currentUser.Id);
			return BadRequest("Folder name is required.");
		}

		if (name.Length > 255)
		{
			_logger.LogWarning("CreateFolder validation failed for UserId={UserId}: name too long ({Length}).", currentUser.Id, name.Length);
			return BadRequest("Folder name must be 255 characters or less.");
		}

		var folder = new Folder
		{
			Name = name,
			OwnerId = currentUser.Id,
			Owner = currentUser
		};

		_dbContext.Folders.Add(folder);
		await _dbContext.SaveChangesAsync();

		_logger.LogInformation("CreateFolder success. FolderId={FolderId}, UserId={UserId}, Name={FolderName}", folder.Id, currentUser.Id, folder.Name);

		return Ok(new
		{
			message = "Folder created successfully",
			folderId = folder.Id
		});
	}

	[HttpPost("{surveyId:guid}/questions")]
	[Authorize]
	public async Task<IActionResult> AddQuestionToSurvey(Guid surveyId, [FromBody] AddSurveyQuestionDto data)
	{
		_logger.LogInformation("AddQuestionToSurvey called. SurveyId={SurveyId}, Authenticated={IsAuthenticated}, UserName={UserName}", surveyId, User.Identity?.IsAuthenticated, User.Identity?.Name);

		if (!User.Identity?.IsAuthenticated ?? true)
		{
			_logger.LogWarning("AddQuestionToSurvey rejected before user lookup: request is not authenticated. SurveyId={SurveyId}", surveyId);
		}

		var currentUser = await _userManager.GetUserAsync(User);
		if (currentUser == null)
		{
			var subject = User.FindFirst("sub")?.Value;
			var userId = User.FindFirst("nameid")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			_logger.LogWarning("AddQuestionToSurvey unauthorized: no matching app user. SurveyId={SurveyId}, sub={Sub}, nameid={NameId}, identityName={IdentityName}", surveyId, subject, userId, User.Identity?.Name);
			return Unauthorized();
		}

		_logger.LogInformation("AddQuestionToSurvey resolved user. SurveyId={SurveyId}, UserId={UserId}", surveyId, currentUser.Id);

		var survey = await _dbContext.Surveys.FindAsync(surveyId);
		if (survey == null)
		{
			_logger.LogWarning("AddQuestionToSurvey failed for UserId={UserId}: SurveyId={SurveyId} not found.", currentUser.Id, surveyId);
			return NotFound("Survey not found.");
		}

		if (survey.OwnerId != currentUser.Id)
		{
			_logger.LogWarning("AddQuestionToSurvey forbidden for UserId={UserId}: SurveyId={SurveyId} owned by {OwnerId}.", currentUser.Id, surveyId, survey.OwnerId);
			return Forbid();
		}

		var name = data.Name.Trim();
		var description = data.Description.Trim();

		if (string.IsNullOrWhiteSpace(name))
		{
			_logger.LogWarning("AddQuestionToSurvey validation failed for UserId={UserId}, SurveyId={SurveyId}: empty question name.", currentUser.Id, surveyId);
			return BadRequest("Question name is required.");
		}

		if (string.IsNullOrWhiteSpace(description))
		{
			_logger.LogWarning("AddQuestionToSurvey validation failed for UserId={UserId}, SurveyId={SurveyId}: empty question description.", currentUser.Id, surveyId);
			return BadRequest("Question description is required.");
		}

		var questionType = await _dbContext.QuestionTypes.FindAsync(data.QuestionTypeId);
		if (questionType == null)
		{
			_logger.LogWarning("AddQuestionToSurvey validation failed for UserId={UserId}, SurveyId={SurveyId}: invalid QuestionTypeId={QuestionTypeId}.", currentUser.Id, surveyId, data.QuestionTypeId);
			return BadRequest("Invalid question type.");
		}

		var questionTemplate = new QuestionTemplate
		{
			Name = name,
			Description = description,
			SurveyId = survey.Id,
			Survey = survey,
			QuestionTypeId = data.QuestionTypeId,
			QuestionType = questionType
		};

		if (data.AnswerOptions != null)
		{
			foreach (var option in data.AnswerOptions)
			{
				var optionDescription = option.Description.Trim();
				if (option.OrderId <= 0 || string.IsNullOrWhiteSpace(optionDescription))
				{
					_logger.LogWarning("AddQuestionToSurvey validation failed for UserId={UserId}, SurveyId={SurveyId}: invalid answer option. OrderId={OrderId}, HasDescription={HasDescription}", currentUser.Id, surveyId, option.OrderId, !string.IsNullOrWhiteSpace(optionDescription));
					return BadRequest("Each answer option needs a positive orderId and a description.");
				}

				questionTemplate.AnswerOptions.Add(new AnswerOption
				{
					OrderId = option.OrderId,
					Description = optionDescription,
					QuestionTemplate = questionTemplate
				});
			}
		}

		_dbContext.QuestionTemplates.Add(questionTemplate);
		await _dbContext.SaveChangesAsync();

		_logger.LogInformation("AddQuestionToSurvey success. SurveyId={SurveyId}, QuestionId={QuestionId}, UserId={UserId}, QuestionTypeId={QuestionTypeId}", surveyId, questionTemplate.Id, currentUser.Id, data.QuestionTypeId);

		return Ok(new
		{
			message = "Question added successfully",
			questionId = questionTemplate.Id
		});
	}
}
