using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using AppyNox.Services.Base.Infrastructure.Localization;

namespace AppyNox.Services.Base.Infrastructure.ExceptionExtensions
{
    internal class CommitException(Exception ex)
        : NoxInfrastructureException(ex, NoxInfrastructureResourceService.CommitException)
    {
    }
}