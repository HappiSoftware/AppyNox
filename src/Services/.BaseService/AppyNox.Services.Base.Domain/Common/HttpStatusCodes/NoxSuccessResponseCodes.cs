using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Domain.Common.HttpStatusCodes
{
    /// <summary>
    /// Successful responses
    /// </summary>
    public enum NoxSuccessResponseCodes
    {
        [Description("The request was successful.")]
        OK = 200,

        [Description("The request was successfully created.")]
        Created = 201,

        [Description("The request has been accepted for processing, but the processing has not been completed.")]
        Accepted = 202,

        [Description("The server successfully processed the request, but there is no content to send back.")]
        NoContent = 204,

        [Description("The server successfully processed the request, but there is no representation to return (i.e., the response is empty).")]
        ResetContent = 205,

        [Description("The server successfully processed only part of the request.")]
        PartialContent = 206,
    }
}