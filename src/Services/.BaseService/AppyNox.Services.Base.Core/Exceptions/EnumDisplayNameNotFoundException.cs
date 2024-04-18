using AppyNox.Services.Base.Core.Common;
using AppyNox.Services.Base.Core.Enums;
using AppyNox.Services.Base.Core.Exceptions.Base;

namespace AppyNox.Services.Base.Core.Exceptions;

internal class EnumDisplayNameNotFoundException(Enum enumValue)
        : NoxException(
            product: ExceptionProduct.AppyNox,
            service: NoxExceptionStrings.Base,
            layer: ExceptionThrownLayer.Core,
            message: $"DisplayName not found for enum '{enumValue}'")
{
}