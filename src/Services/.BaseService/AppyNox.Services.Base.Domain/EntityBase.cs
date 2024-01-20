using AppyNox.Services.Base.Domain.Interfaces;

namespace AppyNox.Services.Base.Domain
{
    public abstract class EntityBase : IEntityWithGuid
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public string Code { get; set; } = string.Empty;

        #endregion
    }
}