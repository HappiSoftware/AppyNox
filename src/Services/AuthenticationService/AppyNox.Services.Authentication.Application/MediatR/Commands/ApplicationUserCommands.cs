using AppyNox.Services.Authentication.Application.DTOs.IdentityUserDTOs.Models;
using MediatR;

namespace AppyNox.Services.Authentication.Application.MediatR.Commands
{
    public record CreateUserCommand(ApplicationUserCreateDto IdentityUserCreateDto) : IRequest<(Guid id, ApplicationUserDto dto)>;
    public record DeleteUserCommand(Guid UserId) : IRequest;
}