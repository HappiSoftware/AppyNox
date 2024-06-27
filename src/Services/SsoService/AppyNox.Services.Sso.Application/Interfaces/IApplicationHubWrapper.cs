namespace AppyNox.Services.Sso.Application.Interfaces;

public interface IApplicationHubWrapper
{
    Task JobStarted(string message);
    Task JobCompleted(string message);
}