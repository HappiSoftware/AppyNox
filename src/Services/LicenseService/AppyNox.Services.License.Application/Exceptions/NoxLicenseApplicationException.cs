﻿using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.License.Domain;

namespace AppyNox.Services.License.Application.Exceptions;

#region [ NoxLicenseApplicationException Code]

internal enum NoxLicenseApplicationExceptionCode
{
    DtoMappingRegistryError = 900,

    AssignKeyCommandError = 1000
}

#endregion

internal class NoxLicenseApplicationException(
    string? message = null,
    int? exceptionCode = null,
    int? statusCode = null,
    Exception? innerException = null)
    : NoxApplicationExceptionBase(
        ExceptionProduct.AppyNox,
        LicenseCommonStrings.Service,
        message,
        exceptionCode,
        statusCode,
        innerException)
{
}