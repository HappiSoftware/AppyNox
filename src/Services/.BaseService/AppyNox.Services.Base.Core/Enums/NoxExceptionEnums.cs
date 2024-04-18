using System.ComponentModel.DataAnnotations;

namespace AppyNox.Services.Base.Core.Enums;

/// <summary>
/// Enumerates the different layers of the application where exceptions are thrown. Used to determine which layer is responsible for the exception.
/// </summary>
public enum ExceptionThrownLayer
{
    /// <summary>
    /// Represents the core layer where exceptions thrown.
    /// </summary>
    [Display(Name = "Core")]
    Core,

    /// <summary>
    /// Represents the domain layer where exceptions thrown.
    /// </summary>
    [Display(Name = "Domain")]
    Domain,

    /// <summary>
    /// Represents the application layer where exceptions thrown.
    /// </summary>
    [Display(Name = "Application")]
    Application,

    /// <summary>
    /// Represents the infrastructure layer where exceptions thrown.
    /// </summary>
    [Display(Name = "Infrastructure")]
    Infrastructure,

    /// <summary>
    /// Represents the API layer where exceptions thrown.
    /// </summary>
    [Display(Name = "Api")]
    Api
}

/// <summary>
/// Enumerates the different products where exceptions are thrown. Used to determine which service is responsible for the exception.
/// </summary>
public enum ExceptionProduct
{
    /// <summary>
    /// Represents the AppyNox service where exceptions thrown.
    /// </summary>
    [Display(Name = "AppyNox")]
    AppyNox,

    /// <summary>
    /// Represents the AppyFleet service where exceptions thrown.
    /// </summary>
    [Display(Name = "AppyFleet")]
    AppyFleet
}