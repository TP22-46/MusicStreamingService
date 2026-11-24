using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStreamingService.Common;
using MusicStreamingService.DTOS;
using MusicStreamingService.Models;
using MusicStreamingService.Services;

namespace MusicStreamingService.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class SongsController : ApiControllerBase
{
    private readonly SongService _songs;

    public SongsController(SongService songs)
    {
        _songs = songs;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string? search, int? moodId)
    {
        var result = await _songs.GetAll(search, moodId);
        return ToResponse(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _songs.GetById(id);
        return ToResponse(result);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create(SaveSongDto dto)
    {
        var result = await _songs.Create(dto);
        return ToResponse(result);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SaveSongDto dto)
    {
        var result = await _songs.Update(id, dto);
        return ToResponse(result);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _songs.Delete(id);
        return ToResponse(result);
    }
}