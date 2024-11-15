namespace AppyNox.Services.Sso.Application.DTOs.AccountDtos.Models;

public class LoginResultDto
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
