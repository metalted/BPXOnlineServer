using FluentResults;
using Metalted.BPX.Authentication.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Metalted.BPX.Authentication;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(
        IAuthenticationService authenticationService,
        ILogger<AuthenticationController> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginResource resource)
    {
        Result<AuthenticationData> result = await _authenticationService.Login(resource);

        if (result.IsFailed)
        {
            _logger.LogError("Failed to authenticate: {Result}", result);
            return Problem("Failed to authenticate", statusCode: 500);
        }

        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public IActionResult Refresh([FromBody] RefreshResource resource)
    {
        Result<AuthenticationData> result = _authenticationService.Refresh(
            resource.SteamId,
            resource.LoginToken,
            resource.RefreshToken);

        if (result.IsFailed)
        {
            _logger.LogError("Failed to refresh token: {Result}", result);
            return Problem("Failed to refresh token", statusCode: 500);
        }

        return Ok(result.Value);
    }
}
