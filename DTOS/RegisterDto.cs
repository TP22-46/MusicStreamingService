using System.ComponentModel.DataAnnotations;
namespace MusicStreamingService.DTOS;

public class RegisterDto
{
     [Required(ErrorMessage = "Имя обязательно")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Телефон обязателен")]
    public string Phone { get; set; } = "";

    [Required]
    [MinLength(4, ErrorMessage = "Пароль не короче 4 символов")]
    public string Password { get; set; } = "";
}
