using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Domain.Common.HttpStatusCodes
{
    /// <summary>
    /// Server error responses
    /// </summary>
    public enum NoxServerErrorResponseCodes
    {
        [Description("The server has encountered a situation it does not know how to handle.")]
        InternalServerError = 500,

        [Description("The request method is not supported by the server and cannot be handled. The only methods that servers are required to support (and therefore that must not return this code) are GET and HEAD.")]
        NotImplemented = 501,

        [Description("This error response means that the server, while working as a gateway to get a response needed to handle the request, got an invalid response.")]
        BadGateway = 502,

        [Description("The server is not ready to handle the request. Common causes are a server that is down for maintenance or that is overloaded. Note that together with this response, a user-friendly page explaining the problem should be sent. This response should be used for temporary conditions, and the Retry-After HTTP header should, if possible, contain the estimated time before the recovery of the service. The webmaster must also take care about the caching-related headers that are sent along with this response, as these temporary condition responses should usually not be cached.")]
        ServiceUnavailable = 503,

        [Description("This error response is given when the server is acting as a gateway and cannot get a response in time.")]
        GatewayTimeout = 504,

        [Description("The HTTP version used in the request is not supported by the server.")]
        HTTPVersionNotSupported = 505,

        [Description("The server has an internal configuration error: the chosen variant resource is configured to engage in transparent content negotiation itself, and is therefore not a proper end point in the negotiation process.")]
        VariantAlsoNegotiates = 506,

        [Description("The method could not be performed on the resource because the server is unable to store the representation needed to successfully complete the request.")]
        InsufficientStorage = 507,

        [Description("The server detected an infinite loop while processing the request.")]
        LoopDetected = 508,

        [Description("Further extensions to the request are required for the server to fulfill it.")]
        NotExtended = 510,

        [Description("Indicates that the client needs to authenticate to gain network access.")]
        NetworkAuthenticationRequired = 511
    }
}