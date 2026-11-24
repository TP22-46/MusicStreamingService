using MusicStreamingService.DTOS;
using MusicStreamingService.Models;

namespace MusicStreamingService.Mappers;

public static class FavoriteMapper
{
    public static SongDto ToSongDto(Favorite favorite)
    {
        return SongMapper.ToDto(favorite.Song!);
    }
}
