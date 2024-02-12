﻿using AppyNox.Services.Sso.Application.ExceptionExtensions;
using AppyNox.Services.Sso.Application.MediatR.Commands;
using AppyNox.Services.Sso.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace AppyNox.Services.Sso.Application.MediatR.Handlers
{
    internal sealed class DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
        : IRequestHandler<DeleteUserCommand>
    {
        #region [ Fields ]

        private readonly UserManager<ApplicationUser> _userManager = userManager;

        #endregion

        #region [ Public Methods ]

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var identityUser = await _userManager.FindByIdAsync(request.UserId.ToString())
                    ?? throw new NoxSsoApplicationException("User Not Found", (int)HttpStatusCode.NotFound);
                await _userManager.DeleteAsync(identityUser);
            }
            catch (Exception ex)
            {
                throw new NoxSsoApplicationException(ex, (int)NoxSsoApplicationExceptionCode.DeleteUserCommandError);
            }
        }

        #endregion
    }
}