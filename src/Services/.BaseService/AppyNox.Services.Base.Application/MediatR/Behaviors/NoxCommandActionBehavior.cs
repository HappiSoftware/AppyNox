using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.Application.Interfaces.MediatR;
using AppyNox.Services.Base.Application.Localization;
using MediatR;

namespace AppyNox.Services.Base.Application.MediatR.Behaviors;

public class NoxCommandActionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IHaveNoxCommandExtensions
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is IHaveNoxCommandExtensions requestWithExtensions && requestWithExtensions.Extensions != null)
        {
            RunAdditionalActions(requestWithExtensions.Extensions, RunType.Before);
        }

        var response = await next();

        if (request is IHaveNoxCommandExtensions requestWithExtensionsAfter && requestWithExtensionsAfter.Extensions != null)
        {
            requestWithExtensionsAfter.Extensions.RunOrderHistory.Add("M"); // add the main action before going to after actions
            RunAdditionalActions(requestWithExtensionsAfter.Extensions, RunType.After);
        }

        return response;
    }

    private static void RunAdditionalActions(NoxCommandExtensions extensions, RunType runType)
    {
        if (extensions == null)
        {
            return;
        }
        if (extensions.Actions.Any(nca => nca.Type == runType))
        {
            string prefixForHistory = runType == RunType.Before ? "B" : "A";
            foreach (var action in extensions.Actions.Where(nca => nca.Type == runType).OrderBy(nca => nca.Order).ToList())
            {
                try
                {
                    extensions.RunOrderHistory.Add($"{prefixForHistory}{action.Order}");
                    action.Action?.Invoke();
                    action.SetStatus(RunStatus.Finished);
                }
                catch (Exception ex)
                {
                    action.SetStatus(RunStatus.ExitedWithError);
                    if (action.SuspendOnFailure)
                    {
                        throw new NoxApplicationException(NoxApplicationResourceService.ProductActionError, action.ExceptionCode, innerException: ex);
                    }
                }
            }
        }
    }
}


