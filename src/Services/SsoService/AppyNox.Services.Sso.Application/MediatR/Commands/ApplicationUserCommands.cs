using AppyNox.Services.Sso.Application.DTOs.ApplicationUserDTOs.Models;
using MediatR;

namespace AppyNox.Services.Sso.Application.MediatR.Commands
{
    public record CreateUserCommand(ApplicationUserCreateDto IdentityUserCreateDto) : IRequest<(Guid id, ApplicationUserDto dto)>;
    public record DeleteUserCommand(Guid UserId) : IRequest;
}