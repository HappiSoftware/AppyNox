using AppyNox.Services.Base.Core.ExceptionExtensions.Base;

namespace AppyNox.Services.Base.Core.ExceptionExtensions
{
    internal class AsyncLocalException(string message)
        : NoxException(message)
    {
    }
}