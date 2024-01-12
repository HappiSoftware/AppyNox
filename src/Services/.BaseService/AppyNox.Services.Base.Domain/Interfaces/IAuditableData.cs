namespace AppyNox.Services.Base.Domain.Interfaces
{
    public interface IAuditableData
    {
        #region [ Properties ]

        string CreatedBy { get; set; }

        DateTime CreationDate { get; set; }

        string UpdatedBy { get; set; }

        DateTime? UpdateDate { get; set; }

        #endregion
    }
}