using AutoWrapper.Wrappers;
using System.Net;
using Xunit;

namespace AppyNox.Services.Base.IntegrationTests.Helpers
{
    /// <summary>
    /// Provides helper methods for validating API responses in integration tests, ensuring that responses meet expected criteria.
    /// </summary>
    public static class ApiResponseHelpers
    {
        #region [ Public Methods ]

        /// <summary>
        /// Asserts that the API response has an OK status code and contains a valid result.
        /// </summary>
        /// <param name="apiResponse">The API response to validate.</param>
        public static void ValidateOk(this ApiResponse apiResponse)
        {
            Assert.NotNull(apiResponse.Result);
            Assert.Equal((int)HttpStatusCode.OK, apiResponse.StatusCode);
        }

        #endregion
    }
}