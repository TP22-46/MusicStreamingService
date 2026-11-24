using System.ComponentModel.DataAnnotations;
namespace MusicStreamingService.DTOS;

public class LoginDto
{
    [Required]
    public string Phone { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}
