﻿using AppyNox.Services.Base.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Infrastructure.Logger
{
    public class NoxInfrastructureLogger(ILogger<NoxInfrastructureLogger> logger) : NoxLogger(logger, "Infrastructure"), INoxInfrastructureLogger
    {
    }
}