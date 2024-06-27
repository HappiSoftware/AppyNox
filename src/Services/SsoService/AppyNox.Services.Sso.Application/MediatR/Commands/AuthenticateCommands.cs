using AppyNox.Services.Sso.Application.DTOs.AccountDtos.Models;
using MediatR;

namespace AppyNox.Services.Sso.Application.MediatR.Commands;


public record AuthenticateCommand(LoginDto UserCredential) : IRequest<AuthenticateResult>;



#region [ Results ]

public record AuthenticateResult(string Token, string RefreshToken);

#endregion