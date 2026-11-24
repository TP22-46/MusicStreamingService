using MusicStreamingService.DTOS;
using MusicStreamingService.Models;

namespace MusicStreamingService.Mappers;

public static class SongMapper
{
    public static SongDto ToDto(Song song) => new()
    {
        Id = song.Id,
        Title = song.Title,
        Artist = song.Artist,
        Genre = song.Genre,
        MoodId = song.MoodId,
        MoodName = song.Mood?.Name ?? "",
        CoverUrl = song.CoverUrl
    };
}
