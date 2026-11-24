using System.ComponentModel.DataAnnotations;

namespace MusicStreamingService.DTOS;

public class SaveSongDto
{
    [Required(ErrorMessage = "Название трека обязательно")]
    [StringLength(40, ErrorMessage = "Не длиннее 40 символов")]
    public string Title { get; set; } = "";

    [Required(ErrorMessage = "Исполнитель обязателен")]
    [StringLength(40, ErrorMessage = "Не длиннее 40 символов")]
    public string Artist { get; set; } = "";

    [Required(ErrorMessage = "Жанр обязателен")]
    [StringLength(30, ErrorMessage = "Не длиннее 30 символов")]
    public string Genre { get; set; } = "";

    [Range(1, int.MaxValue, ErrorMessage = "Укажите номер категории")]
    public int MoodId { get; set; }

    public string? CoverUrl { get; set; }
}
