namespace AppyNox.Services.Authentication.Application.Dtos
{
    /// <summary>
    /// Interface defining a globally unique identifier (GUID) property.
    /// </summary>
    public interface IHasGuid
    {
        #region [ Properties ]

        string Id { get; set; }

        #endregion
    }
}