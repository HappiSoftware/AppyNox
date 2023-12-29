using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Base.Domain.Common
{
    /// <summary>
    /// Specifies the types of Data Transfer Object (DTO) level mappings.
    /// </summary>
    public enum DtoLevelMappingTypes
    {
        /// <summary>
        /// Represents data access level DTO mapping.
        /// </summary>
        [Display(Name = "DataAccess")]
        DataAccess,

        /// <summary>
        /// Represents DTO mapping for creating new entities.
        /// </summary>
        [Display(Name = "Create")]
        Create,

        /// <summary>
        /// Represents DTO mapping for updating existing entities.
        /// </summary>
        [Display(Name = "Update")]
        Update
    }
}