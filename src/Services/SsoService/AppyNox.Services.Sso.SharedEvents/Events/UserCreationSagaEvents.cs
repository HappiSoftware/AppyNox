namespace AppyNox.Services.Sso.SharedEvents.Events
{
    public record StartUserCreationMessage(Guid CorrelationId, string LicenseKey, string UserName, string Email, string Password, string ConfirmPassword);
    public record CreateApplicationUserMessage(Guid CorrelationId, string UserName, string Email, string Password, string ConfirmPassword, Guid CompanyId);
    public record ApplicationUserCreatedEvent(Guid CorrelationId, Guid UserId);
    public record DeleteApplicationUserMessage(Guid CorrelationId, Guid UserId);
}