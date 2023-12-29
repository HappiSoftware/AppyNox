using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.API.Handlers
{
    public class CorrelationIdDelegatingHandler : DelegatingHandler
    {
        #region [ Protected Methods ]

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains("X-Correlation-ID"))
            {
                request.Headers.Add("X-Correlation-ID", Guid.NewGuid().ToString());
            }

            return await base.SendAsync(request, cancellationToken);
        }

        #endregion
    }
}