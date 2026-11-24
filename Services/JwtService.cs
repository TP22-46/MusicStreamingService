using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MusicStreamingService.Models;

namespace MusicStreamingService.Services;

public class JwtService
{
    private readonly string _key;

    public JwtService(IConfiguration config)
    {
        _key = config["Jwt:Key"]
            ?? throw new InvalidOperationException("Нет Jwt:Key");
    }

    public string CreateToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
        };

    var credentials = new SigningCredentials(
        new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_key)),
        SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.UtcNow.AddDays(7),
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler()
        .WriteToken(token);
    }
}
