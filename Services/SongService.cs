using Microsoft.EntityFrameworkCore;
using MusicStreamingService.Common;
using MusicStreamingService.Data;
using MusicStreamingService.DTOS;
using MusicStreamingService.Mappers;
using MusicStreamingService.Models;

namespace MusicStreamingService.Services;

public class SongService
{
    private readonly AppDbContext _db;

    public SongService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result> GetAll(string? search, int? moodId)
    {
        var query = _db.Songs
            .Include(s => s.Mood)
            .AsQueryable();

        if (moodId != null)
            query = query.Where(s => s.MoodId == moodId);

        var songs = await query.ToListAsync();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var text = search.Trim();
            songs = songs
                .Where(s => s.Title.Contains(text, StringComparison.OrdinalIgnoreCase)
                         || s.Artist.Contains(text, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        var dtos = songs.Select(SongMapper.ToDto).ToList();
        return Result.Ok(dtos);
    }

    public async Task<Result> GetById(int id)
    {
        var song = await _db.Songs
            .Include(s => s.Mood)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (song == null)
            return Result.NotFound("Песня не найдена");

        return Result.Ok(SongMapper.ToDto(song));
    }

    public async Task<Result> Create(SaveSongDto dto)
    {
        var mood = await _db.Moods.FindAsync(dto.MoodId);
        if (mood == null)
            return Result.BadRequest("Настроение не найдено");

        var song = new Song
        {
            Title = dto.Title,
            Artist = dto.Artist,
            Genre = dto.Genre,
            MoodId = dto.MoodId,
            Mood = mood,
            CoverUrl = dto.CoverUrl
        };

        _db.Songs.Add(song);
        await _db.SaveChangesAsync();

        return Result.Created(SongMapper.ToDto(song));
    }

    public async Task<Result> Update(int id, SaveSongDto dto)
    {
        var song = await _db.Songs.FindAsync(id);
        if (song == null)
            return Result.NotFound("Песня не найдена");

        var mood = await _db.Moods.FindAsync(dto.MoodId);
        if (mood == null)
            return Result.BadRequest("Настроение не найдено");

        song.Title = dto.Title;
        song.Artist = dto.Artist;
        song.Genre = dto.Genre;
        song.MoodId = dto.MoodId;
        song.Mood = mood;
        song.CoverUrl = dto.CoverUrl;

        await _db.SaveChangesAsync();

        return Result.Ok(SongMapper.ToDto(song));
    }

    public async Task<Result> Delete(int id)
    {
        var song = await _db.Songs.FindAsync(id);
        if (song == null)
            return Result.NotFound("Песня не найдена");

        _db.Songs.Remove(song);
        await _db.SaveChangesAsync();

        return Result.NoContent();
    }
}