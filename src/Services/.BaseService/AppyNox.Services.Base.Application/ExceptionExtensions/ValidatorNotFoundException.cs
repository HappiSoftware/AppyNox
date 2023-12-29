using AppyNox.Services.Base.Application.ExceptionExtensions.Base;
using System.Net;

namespace AppyNox.Services.Base.Application.ExceptionExtensions
{
    /// <summary>
    /// Exception thrown when no validator is found for a given DTO type.
    /// </summary>
    internal class ValidatorNotFoundException(Type dtoType)
        : NoxApplicationException($"No validator found for '{dtoType}'.")
    {
    }
}