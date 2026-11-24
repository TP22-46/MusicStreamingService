using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStreamingService.Common;
using MusicStreamingService.Services;

namespace MusicStreamingService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ApiControllerBase
{
    private readonly FavoriteService _favorites;

    public FavoritesController(FavoriteService favorites)
    {
        _favorites = favorites;
    }

    private int GetUserId()
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) 
                      ?? User.FindFirstValue("id");
        return int.Parse(idClaim!);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyFavorites()
    {
        var result = await _favorites.GetMyFavorites(GetUserId());
        return ToResponse(result);
    }

    [HttpPost("{songId}")]
    public async Task<IActionResult> Add(int songId)
    {
        var result = await _favorites.Add(GetUserId(), songId);
        return ToResponse(result);
    }

    [HttpDelete("{songId}")]
    public async Task<IActionResult> Remove(int songId)
    {
        var result = await _favorites.Remove(GetUserId(), songId);
        return ToResponse(result);
    }
}
