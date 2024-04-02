using AppyNox.Services.Base.API.Controllers;
using AppyNox.Services.Sso.Domain.Entities;
using AppyNox.Services.Sso.Infrastructure.Data;
using AppyNox.Services.Sso.WebAPI.ExceptionExtensions.Base;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppyNox.Services.Sso.WebAPI.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/email-providers")]
public class EmailProvidersController(IdentityDatabaseContext dbContext) : NoxController
{
    private readonly IdentityDatabaseContext _dbContext = dbContext;

    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetEmailProvider([FromRoute] Guid companyId)
    {
        var companyWithEmailProvider = await _dbContext.Companies
            .Where(c => c.Id == companyId)
            .Select(c => new
            {
                Company = c,
                EmailProvider = c.EmailProviders.Where(ep => ep.Active).FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        EmailProvider? activeEmailProvider = null;
        if (companyWithEmailProvider != null && companyWithEmailProvider.EmailProvider != null)
        {
            activeEmailProvider = companyWithEmailProvider.EmailProvider;
        }

        return Ok(activeEmailProvider ?? throw new NoxSsoApiException("EmailProvider could not found.", (int)NoxSsoApiExceptionCode.SsoServiceApiError));
    }
}