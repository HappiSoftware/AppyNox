namespace AppyNox.Services.Sso.SharedEvents.Events
{
    public record StartUserCreationMessage(Guid CorrelationId, string LicenseKey, string UserName, string Email, string Password, string ConfirmPassword, string Name, string Surname, string Code);
    public record CreateApplicationUserMessage(Guid CorrelationId, string UserName, string Email, string Password, string ConfirmPassword, Guid CompanyId, string Name, string Surname, string Code);
    public record ApplicationUserCreatedEvent(Guid CorrelationId, Guid UserId);
    public record DeleteApplicationUserMessage(Guid CorrelationId, Guid UserId);
}