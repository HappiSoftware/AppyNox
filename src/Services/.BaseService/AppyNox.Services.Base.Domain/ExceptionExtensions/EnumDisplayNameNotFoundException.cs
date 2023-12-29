using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using System.Net;

namespace AppyNox.Services.Base.Domain.ExceptionExtensions
{
    internal class EnumDisplayNameNotFoundException(Enum enumValue)
        : NoxException($"DisplayName not found for enum '{enumValue}'")
    {
    }
}