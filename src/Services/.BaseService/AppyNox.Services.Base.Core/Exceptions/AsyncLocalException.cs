using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Exceptions.Base;

namespace AppyNox.Services.Base.Core.Exceptions;

internal class AsyncLocalException(string message)
        : NoxException(
            product: ExceptionProduct.AppyNox,
            service: NoxExceptionStrings.Base,
            layer: ExceptionThrownLayer.Core,
            message: message)
{
}