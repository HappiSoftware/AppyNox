using AppyNox.Services.Authentication.Application.Dtos.IdentityUserDtos.Models.Base;
using MediatR;

namespace AppyNox.Services.Authentication.Application.MediatR.Commands
{
    public class CreateUserCommand(IdentityUserCreateDto dto) : IRequest<(Guid id, IdentityUserDto dto)>
    {
        #region Properties

        public IdentityUserCreateDto IdentityUserCreateDto { get; set; } = dto;

        #endregion
    }
}