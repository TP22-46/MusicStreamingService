using Microsoft.EntityFrameworkCore;
using MusicStreamingService.Common;
using MusicStreamingService.Data;
using MusicStreamingService.Mappers;
using MusicStreamingService.Models;

namespace MusicStreamingService.Services;

public class FavoriteService
{
    private readonly AppDbContext _db;

    public FavoriteService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result> GetMyFavorites(int userId)
    {
        var favorites = await _db.Favorites
            .Where(f => f.UserId == userId)
            .Include(f => f.Song)
                .ThenInclude(s => s!.Mood)
            .OrderByDescending(f => f.AddedAt)
            .ToListAsync();

        var dtos = favorites.Select(FavoriteMapper.ToSongDto).ToList();
        return Result.Ok(dtos);
    }

    public async Task<Result> Add(int userId, int songId)
    {
        var song = await _db.Songs
            .Include(s => s.Mood)
            .FirstOrDefaultAsync(s => s.Id == songId);

        if (song == null)
            return Result.NotFound("Песня не найдена");

        var alreadyAdded = await _db.Favorites
            .AnyAsync(f => f.UserId == userId && f.SongId == songId);

        if (alreadyAdded)
            return Result.Conflict("Эта песня уже в избранном");

        var favorite = new Favorite
        {
            UserId = userId,
            SongId = songId,
            Song = song
        };

        _db.Favorites.Add(favorite);
        await _db.SaveChangesAsync();

        return Result.Created(FavoriteMapper.ToSongDto(favorite));
    }

    public async Task<Result> Remove(int userId, int songId)
    {
        var favorite = await _db.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.SongId == songId);

        if (favorite == null)
            return Result.NotFound("Песня не найдена в вашем избранном");

        _db.Favorites.Remove(favorite);
        await _db.SaveChangesAsync();

        return Result.NoContent();
    }
}