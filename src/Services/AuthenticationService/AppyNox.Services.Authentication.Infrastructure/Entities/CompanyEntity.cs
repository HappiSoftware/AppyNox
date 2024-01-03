namespace AppyNox.Services.Authentication.Domain.Entities
{
    public class CompanyEntity
    {
        #region [ Properties ]

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public virtual ICollection<ApplicationUser>? Users { get; set; }

        #endregion
    }
}