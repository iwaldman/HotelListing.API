using HotelListing.API.Contracts;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    // Dependency Injected
    private readonly IAuthManager _authManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
    {
        _authManager = authManager;
        _logger = logger;
    }

    // POST: api/account/register
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
    {
        _logger.LogInformation($"Registration Attempt for {apiUserDto.Email}");

        try
        {
            var errors = await _authManager.Register(apiUserDto);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            // log exception
            _logger.LogError(
                ex,
                $"Something went wrong in the {nameof(Register)} - {apiUserDto.Email}"
            );

            return Problem(
                $"Something went wrong in the {nameof(Register)}",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    // POST: api/account/login
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {
        _logger.LogInformation($"Login Attempt for {loginDto.Email}");

        try
        {
            var authResponse = await _authManager.Login(loginDto);

            if (authResponse is null)
            {
                return Unauthorized();
            }

            return Ok(authResponse);
        }
        catch (Exception ex)
        {
            // log exception
            _logger.LogError(ex, $"Something went wrong in the {nameof(Login)} - {loginDto.Email}");

            return Problem(
                $"Something went wrong in the {nameof(Login)}",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    // POST: api/account/refresh
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Refresh([FromBody] AuthResponseDto authResponseDto)
    {
        _logger.LogInformation($"Refresh Attempt for {authResponseDto.UserId}");

        try
        {
            var authResponse = await _authManager.VerifyRefreshToken(authResponseDto);

            if (authResponse is null)
            {
                return Unauthorized();
            }

            return Ok(authResponse);
        }
        catch (Exception ex)
        {
            // log exception
            _logger.LogError(
                ex,
                $"Something went wrong in the {nameof(Refresh)} - {authResponseDto.UserId}"
            );

            return Problem(
                $"Something went wrong in the {nameof(Refresh)}",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
}
