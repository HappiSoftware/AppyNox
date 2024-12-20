﻿using AppyNox.Services.Base.API.Exceptions.Base;
using AppyNox.Services.Base.API.Localization;
using System.Net;

namespace AppyNox.Services.Base.API.Exceptions;

/// <summary>
/// Exception thrown when a required correlation ID is missing in an API request.
/// </summary>
internal class MissingCorrelationIdException()
    : NoxApiException(
        NoxApiResourceService.CorrelationIdIsRequired,
        (int)NoxApiExceptionCode.CorrelationIdError,
        (int)HttpStatusCode.BadRequest)
{
}