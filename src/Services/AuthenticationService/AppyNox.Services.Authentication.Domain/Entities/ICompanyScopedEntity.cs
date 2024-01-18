using AppyNox.Services.Base.Domain;

namespace AppyNox.Services.Authentication.Domain.Entities
{
    public interface ICompanyScopedEntity
    {
        #region [ Properties ]

        string Code { get; set; }

        Guid CompanyId { get; set; }

        #endregion
    }
}