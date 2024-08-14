using System.Security.Claims;
using FluentResults;
using Metalted.BPX.Blueprints.Resources;
using Metalted.BPX.Data.Entities;
using Metalted.BPX.Jwt;
using Metalted.BPX.Storage;
using Metalted.BPX.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Metalted.BPX.Blueprints;

[ApiController]
[Route("blueprints")]
public class BlueprintController : ControllerBase
{
    private readonly IBlueprintService _blueprintService;
    private readonly IStorageService _storageService;
    private readonly IUserService _userService;
    private readonly ILogger<BlueprintController> _logger;

    public BlueprintController(
        IBlueprintService blueprintService,
        IStorageService storageService,
        IUserService userService,
        ILogger<BlueprintController> logger)
    {
        _blueprintService = blueprintService;
        _storageService = storageService;
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("download/blueprint")]
    [AllowAnonymous]
    public async Task<IActionResult> Download(int userId, string fileId)
    {
        if (!_userService.TryGet(userId, out User? user))
        {
            return NotFound();
        }

        Result<bool> exists = await _storageService.Exists(user.Id, fileId);
        if (exists.IsFailed)
        {
            _logger.LogError("Failed to check if blueprint exists: {Result}", exists);
            return Problem("Failed to check if blueprint exists", statusCode: 500);
        }
        
        if (!exists.Value)
        {
            return NotFound();
        }

        Result<byte[]> download = await _storageService.DownloadBlueprint(user.Id, fileId);
        if (download.IsFailed)
        {
            _logger.LogError("Failed to download blueprint: {Result}", download);
            return Problem("Failed to download blueprint", statusCode: 500);
        }

        return File(download.Value, "application/octet-stream", fileId + ".blueprint");
    }

    [HttpGet("download/image")]
    [AllowAnonymous]
    public async Task<IActionResult> DownloadImage(int userId, string fileId)
    {
        if (!_userService.TryGet(userId, out User? user))
        {
            return NotFound();
        }

        Result<bool> exists = await _storageService.Exists(user.Id, fileId);
        if (exists.IsFailed)
        {
            _logger.LogError("Failed to check if image exists: {Result}", exists);
            return Problem("Failed to check if image exists", statusCode: 500);
        }
        
        if (!exists.Value)
        {
            return NotFound();
        }

        Result<byte[]> download = await _storageService.DownloadImage(user.Id, fileId);
        if (download.IsFailed)
        {
            _logger.LogError("Failed to download image: {Result}", download);
            return Problem("Failed to download image", statusCode: 500);
        }

        return File(download.Value, "image/png", fileId + ".png");
    }

    [HttpGet("exists")]
    public IActionResult Exists(string name)
    {
        string? value = User.FindFirstValue(IJwtService.SteamIdClaimName);
        if (string.IsNullOrEmpty(value))
        {
            return Unauthorized();
        }

        if (!ulong.TryParse(value, out ulong steamId))
        {
            return Unauthorized();
        }

        Result<bool> result = _blueprintService.Exists(steamId, name);
        if (!result.IsFailed)
            return Ok(result.Value);

        _logger.LogError("Failed to check if blueprint exists: {Result}", result);
        return Problem("Failed to check if blueprint exists", statusCode: 500);
    }

    [HttpGet("latest/{amount}")]
    public IActionResult Latest(int amount)
    {
        return Ok(_blueprintService.Latest(amount));
    }

    [HttpPost("search")]
    public IActionResult Search([FromBody] SearchResource resource)
    {
        return Ok(_blueprintService.Search(resource));
    }

    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] BlueprintResource resource)
    {
        string? value = User.FindFirstValue(IJwtService.SteamIdClaimName);
        if (string.IsNullOrEmpty(value))
        {
            return Unauthorized();
        }

        if (!ulong.TryParse(value, out ulong steamId))
        {
            return Unauthorized();
        }

        Result<Blueprint> result = await _blueprintService.Submit(steamId, resource);
        if (!result.IsFailed)
            return Ok(result.Value);

        _logger.LogError("Failed to submit blueprint: {Result}", result);
        return Problem("Failed to submit blueprint", statusCode: 500);
    }
}
