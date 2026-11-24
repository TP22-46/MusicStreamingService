using System.Security.Claims;
using MusicStreamingService.Models;
using Microsoft.AspNetCore.Mvc;
using MusicStreamingService.Common;

namespace MusicStreamingService.Common;

public abstract class ApiControllerBase : ControllerBase
{
    protected int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    protected bool IsAdmin => User.IsInRole(Roles.Admin);

    protected IActionResult ToResponse(Result result)
    {
        if (!result.Success)
            return StatusCode(result.StatusCode, new { message = result.Message });

        if (result.StatusCode == 204)
            return NoContent();

        return StatusCode(result.StatusCode, result.Data);
    }
}
