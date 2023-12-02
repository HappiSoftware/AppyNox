using AppyNox.Services.Base.Domain.ExceptionExtensions.Base;
using AppyNox.Services.Base.Domain.ExceptionExtensions.Enums;

namespace AppyNox.EventBus.Base.ExceptionExtensions.Base;

public class EventBusBaseException(string message, int statusCode)
        : NoxException(ExceptionThrownLayer.InfrastructureBase, message, statusCode)
{
}
