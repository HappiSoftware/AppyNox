using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.API.Logger
{
    public class NoxApiLogger(ILogger<INoxApiLogger> logger) : NoxLogger(logger, "Api"), INoxApiLogger
    {
    }
}