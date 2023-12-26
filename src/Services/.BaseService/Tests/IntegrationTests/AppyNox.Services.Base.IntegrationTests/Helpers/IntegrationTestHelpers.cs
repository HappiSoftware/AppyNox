using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.IntegrationTests.Helpers
{
    public static class IntegrationTestHelpers
    {
        #region [ Public Methods ]

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