using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Base.Domain.ExceptionExtensions.Enums
{
    /// <summary>
    /// Enumerates the different layers of the application where exceptions are thrown. Used to determine which layer is responsible for the exception.
    /// </summary>
    public enum ExceptionThrownLayer
    {
        /// <summary>
        /// Represents the domain base layer where exceptions thrown.
        /// </summary>
        [Display(Name = "Nox Domain Base")]
        DomainBase,

        /// <summary>
        /// Represents the application base layer where exceptions thrown.
        /// </summary>
        [Display(Name = "Nox Application Base")]
        ApplicationBase,

        /// <summary>
        /// Represents the infrastructure base layer where exceptions thrown.
        /// </summary>
        [Display(Name = "Nox Infrastructure Base")]
        InfrastructureBase,

        /// <summary>
        /// Represents the API base layer where exceptions thrown.
        /// </summary>
        [Display(Name = "Nox Api Base")]
        ApiBase
    }
}