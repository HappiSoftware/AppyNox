namespace AppyNox.Services.Sso.Domain.Entities;

public class EmailProvider
{
    public Guid Id { get; set; }

    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool UseSSL { get; set; }

    public string FromAddress { get; set; } = string.Empty;

    public string FromName { get; set; } = string.Empty;

    public bool Active { get; set; }

    public Guid CompanyId { get; set; }
}