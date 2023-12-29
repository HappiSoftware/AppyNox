using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Base.Domain.Common
{
    /// <summary>
    /// Specifies different levels of data transfer object (DTO) representation.
    /// </summary>
    public enum CommonDtoLevelEnums
    {
        /// <summary>
        /// Represents no specific DTO level.
        /// </summary>
        [Display(Name = "None")]
        None,

        /// <summary>
        /// Represents a simple DTO level, typically with essential fields. Most basic DTO level.
        /// </summary>
        [Display(Name = "Simple")]
        Simple,

        /// <summary>
        /// Represents a DTO level with only identifiers.
        /// </summary>
        [Display(Name = "IdOnly")]
        IdOnly
    }
}