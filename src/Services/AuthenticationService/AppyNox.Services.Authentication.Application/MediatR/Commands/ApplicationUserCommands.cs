using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base;
using MediatR;

namespace AppyNox.Services.Authentication.Application.MediatR.Commands
{
    public record CreateUserCommand(IdentityUserCreateDto IdentityUserCreateDto) : IRequest<(Guid id, IdentityUserDto dto)>;
    public record DeleteUserCommand(Guid UserId) : IRequest;
}