using System.Text.RegularExpressions;
using Backend.Dto;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<UserController> _logger;

    public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<UserController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDto data)
    {
        var user = new User
        {
            UserName = data.Username,
            Email = data.Email,
        };

        var result = await _userManager.CreateAsync(user, data.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        _logger.LogInformation("New User created");

        await _signInManager.SignInAsync(user, isPersistent: false);
        return Ok(new { message = "Registration Successful" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] UserLoginDto data)
    {
        var user = await _userManager.FindByEmailAsync(data.Email);
        if (user == null)
        {
            return BadRequest("Incorrect E-Mail or Password");
        }

        var result = _signInManager.PasswordSignInAsync(
            user,
            data.Password,
            isPersistent: data.StaySignedIn,
            lockoutOnFailure: true
        );

        if (result.Result.Succeeded)
        {
            await _signInManager.SignInAsync(user, data.StaySignedIn);
            return Ok(new { message = "Login Successful" });
        }

        if (result.Result.IsLockedOut)
        {
            return BadRequest("This account was temporarily locked because of too many failed Sign-In requests");
        }

        return BadRequest("Incorrect E-Mail or Password");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { message = "Successfully logged out" });
    }

    [HttpGet("checkUsername")]
    [ProducesResponseType(typeof(UserUsernameAvailabilityResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserUsernameAvailabilityResponseDto>> CheckUsername([FromQuery] UserUsernameCheckRequestDto request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user != null)
        {
            return Ok(new UserUsernameAvailabilityResponseDto
            {
                IsAvailable = false,
                Message = "User with this name already exists"
            });
        }

        return Ok(new UserUsernameAvailabilityResponseDto
        {
            IsAvailable = true,
            Message = "Username is available"
        });
    }

    [HttpPost("checkEmail")]
    public async Task<IActionResult> CheckEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null)
        {
            return BadRequest("User with E-Mail already exists");
        }

        return Ok(new { message = "E-Mail is available" });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserAuthDto>> GetCurrentUser()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var userData = new UserAuthDto
        {
            Id = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            ProfilePictureUrl = user.ProfilePictureUrl
        };

        return Ok(userData);
    }

    [HttpPatch("username")]
    [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> ChangeUsername([FromBody] string newUsername)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        newUsername = newUsername.Trim();

        var res = await _userManager.SetUserNameAsync(user, newUsername);

        if (!res.Succeeded)
        {
            return Conflict(res.Errors);
        }

        return Ok(newUsername);
    }

    [HttpPatch("email")]
    [ProducesResponseType(typeof(IEnumerable<IdentityError>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [Authorize]
    public async Task<IActionResult> ChangeEmail([FromBody] string newEmail)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        newEmail = newEmail.Trim();

        var res = await _userManager.SetEmailAsync(user, newEmail);

        if (!res.Succeeded)
        {
            return Conflict(res.Errors);
        }

        return Ok(newEmail);
    }
}