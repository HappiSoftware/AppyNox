namespace AppyNox.Services.Authentication.Application.Dtos
{
    /// <summary>
    /// Data transfer object with a globally unique identifier (GUID).
    /// Implements the IHasGuid interface.
    /// </summary>
    public class GuidDto : IHasGuid
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        #endregion
    }
}