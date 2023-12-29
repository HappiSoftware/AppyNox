using AppyNox.Services.Base.API.ExceptionExtensions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.API.ExceptionExtensions
{
    internal class MissingCorrelationIdException(string message) : NoxApiException(message, (int)HttpStatusCode.BadRequest)
    {
    }
}