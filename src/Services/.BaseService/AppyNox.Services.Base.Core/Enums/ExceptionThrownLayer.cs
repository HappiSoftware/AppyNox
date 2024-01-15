using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Base.Core.Enums
{
    /// <summary>
    /// Enumerates the different layers of the application where exceptions are thrown. Used to determine which layer is responsible for the exception.
    /// </summary>
    public enum ExceptionThrownLayer
    {
        /// <summary>
        /// Represents the domain layer where exceptions thrown.
        /// </summary>
        [Display(Name = "Nox Domain")]
        Domain,

        /// <summary>
        /// Represents the application layer where exceptions thrown.
        /// </summary>
        [Display(Name = "Nox Application")]
        Application,

        /// <summary>
        /// Represents the infrastructure layer where exceptions thrown.
        /// </summary>
        [Display(Name = "Nox Infrastructure")]
        Infrastructure,

        /// <summary>
        /// Represents the API layer where exceptions thrown.
        /// </summary>
        [Display(Name = "Nox Api")]
        Api
    }
}