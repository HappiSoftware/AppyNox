using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Domain.Common.HttpStatusCodes
{
    /// <summary>
    /// Redirection messages
    /// </summary>
    public enum NoxRedirectionResponseCodes
    {
        [Description("The requested resource has multiple choices, and the user or user agent can select the one desired.")]
        MultipleChoices = 300,

        [Description("The requested page has moved permanently to a new location.")]
        MovedPermanently = 301,

        [Description("The requested page has moved temporarily to a new location.")]
        Found = 302,

        [Description("The response to the request can be found under another URI using a GET method.")]
        SeeOther = 303,

        [Description("The server has not found anything matching the requested URI.")]
        NotModified = 304,

        [Description("The requested page can be accessed from the proxy but it has not been modified since the last request.")]
        UseProxy = 305, // Not widely used, but included for completeness

        [Description("The requested page has moved temporarily to a new location, but the change is intended to be temporary.")]
        TemporaryRedirect = 307,

        [Description("The request and all future requests should be repeated using another URI.")]
        PermanentRedirect = 308,
    }
}