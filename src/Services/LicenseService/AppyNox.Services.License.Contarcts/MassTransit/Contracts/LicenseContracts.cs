namespace AppyNox.Services.License.Contarcts.MassTransit.Contracts;

#region [ License ]

/// <summary>
/// Fetches License By Key
/// </summary>
public record GetLicenseIdByKeyDataRequest(string LicenseKey);

public record GetLicenseIdByKeyDataResponse(Guid LicenseId);

#endregion