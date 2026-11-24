namespace MusicStreamingService.Models;

public class Song
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Artist { get; set; } = "";
    public string Genre { get; set; } = "";

    public int MoodId { get; set; }
    public Mood? Mood { get; set; }

    public string? CoverUrl { get; set; } = "";
}
