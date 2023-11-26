using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Domain.Common.HttpStatusCodes
{
    /// <summary>
    /// Client error responses
    /// </summary>
    public enum NoxClientErrorResponseCodes
    {
        [Description("The server cannot or will not process the request due to an apparent client error.")]
        BadRequest = 400,

        [Description("Similar to 403 Forbidden, but specifically for authentication.")]
        Unauthorized = 401,

        [Description("Reserved for future use.")]
        PaymentRequired = 402,

        [Description("The client does not have the necessary permission.")]
        Forbidden = 403,

        [Description("The server cannot find the requested resource.")]
        NotFound = 404,

        [Description("The request method is not allowed for the requested resource.")]
        MethodNotAllowed = 405,

        [Description("The server cannot produce a response matching the list of acceptable values defined in the request's headers.")]
        NotAcceptable = 406,

        [Description("The client must first authenticate itself with the proxy.")]
        ProxyAuthenticationRequired = 407,

        [Description("The server timed out waiting for the request.")]
        RequestTimeout = 408,

        [Description("Indicates that the request could not be processed because of conflict in the current state of the target resource.")]
        Conflict = 409,

        [Description("The requested resource is no longer available and will not be available again.")]
        Gone = 410,

        [Description("The request did not specify the length of its content, which is required by the requested resource.")]
        LengthRequired = 411,

        [Description("The server does not meet one of the preconditions specified in the request headers.")]
        PreconditionFailed = 412,

        [Description("The request is larger than the server is willing or able to process.")]
        PayloadTooLarge = 413,

        [Description("The URI provided was too long for the server to process.")]
        URITooLong = 414,

        [Description("The server does not support the media type that the client specified in the request.")]
        UnsupportedMediaType = 415,

        [Description("The server cannot provide the requested range.")]
        RangeNotSatisfiable = 416,

        [Description("The server cannot meet the requirements of the Expect request-header field.")]
        ExpectationFailed = 417,

        [Description("The request was directed at a server that is not able to produce a response.")]
        MisdirectedRequest = 421,

        [Description("The request was well-formed but was unable to be followed due to semantic errors.")]
        UnprocessableEntity = 422,

        [Description("The resource that is being accessed is locked.")]
        Locked = 423,

        [Description("The request failed because it depended on another request and that request failed.")]
        FailedDependency = 424,

        [Description("The client should switch to a different protocol.")]
        UpgradeRequired = 426,

        [Description("The origin server requires the request to be conditional.")]
        PreconditionRequired = 428,

        [Description("The user has sent too many requests in a given amount of time.")]
        TooManyRequests = 429,

        [Description("The server is unwilling to process the request because its header fields are too large.")]
        RequestHeaderFieldsTooLarge = 431,

        [Description("A server operator has received a legal demand to deny access to a resource or to a set of resources.")]
        UnavailableForLegalReasons = 451
    }
}