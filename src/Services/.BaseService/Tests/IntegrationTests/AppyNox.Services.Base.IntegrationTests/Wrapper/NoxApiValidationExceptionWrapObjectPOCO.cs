using FluentValidation.Results;

namespace AppyNox.Services.Base.IntegrationTests.Wrapper;

public class NoxApiValidationExceptionWrapObjectPOCO : NoxApiExceptionWrapObjectPOCO
{
    #region [ Properties ]

    public IEnumerable<ValidationFailure> ValidationErrors { get; set; } = default!;

    #endregion
}