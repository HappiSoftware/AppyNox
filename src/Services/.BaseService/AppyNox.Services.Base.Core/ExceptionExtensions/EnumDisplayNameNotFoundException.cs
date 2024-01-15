using AppyNox.Services.Base.Core.ExceptionExtensions.Base;
using System.Net;

namespace AppyNox.Services.Base.Core.ExceptionExtensions
{
    internal class EnumDisplayNameNotFoundException(Enum enumValue)
        : NoxException($"DisplayName not found for enum '{enumValue}'")
    {
    }
}