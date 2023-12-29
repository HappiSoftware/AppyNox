using Microsoft.Extensions.Configuration;

namespace AppyNox.Services.Base.IntegrationTests.Helpers
{
    /// <summary>
    /// Provides utility methods for integration testing, including configuration retrieval and environment setup.
    /// </summary>
    public static class IntegrationTestHelpers
    {
        #region [ Public Methods ]

        /// <summary>
        /// Retrieves the configuration settings for integration tests.
        /// </summary>
        /// <returns>The configuration root containing test settings.</returns>
        public static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("serviceuris.json", optional: false, reloadOnChange: true)
                .Build();
        }

        #endregion
    }
}