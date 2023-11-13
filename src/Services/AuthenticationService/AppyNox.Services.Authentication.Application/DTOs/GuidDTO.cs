namespace AppyNox.Services.Authentication.Application.Dtos
{
    public class GuidDto : IHasGuid
    {
        #region [ Properties ]

        public string Id { get; set; } = string.Empty;

        #endregion
    }
}