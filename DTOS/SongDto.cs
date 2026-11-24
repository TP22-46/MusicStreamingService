namespace MusicStreamingService.DTOS;

public class SongDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Artist { get; set; } = "";
    public string Genre { get; set; } = "";
    public int MoodId { get; set; }
    public string MoodName { get; set; } = "";
    public string? CoverUrl { get; set; }
}