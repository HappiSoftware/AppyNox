using AppyNox.Services.Base.Domain.Common.HttpStatusCodes;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;
using AppyNox.Services.Base.Infrastructure.ExceptionExtensions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Infrastructure.ExceptionExtensions
{
    internal class CommitException(Exception ex)
        : NoxInfrastructureException(ex, "An error occurred while saving changes to the database.")
    {
    }
}