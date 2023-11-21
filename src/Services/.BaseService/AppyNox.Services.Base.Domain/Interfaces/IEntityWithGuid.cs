namespace AppyNox.Services.Base.Domain.Interfaces
{
    public interface IEntityWithGuid
    {
        #region [ Properties ]

        Guid Id { get; set; }

        #endregion
    }
}