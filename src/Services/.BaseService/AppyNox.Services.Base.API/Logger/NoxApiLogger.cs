using AppyNox.Services.Base.Infrastructure.Logger;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Infrastructure.Services.LoggerService
{
    public class NoxApiLogger(ILogger<NoxApiLogger> logger) : NoxLogger(logger, "Api"), INoxApiLogger
    {
    }
}