using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;

namespace AppyNox.Services.Base.Infrastructure.ExceptionExtensions
{
    internal class CommitException(Exception ex)
        : NoxInfrastructureException(ex, "An error occurred while saving changes to the database.")
    {
    }
}