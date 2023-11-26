using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Base.Domain.Common.HttpStatusCodes
{
    /// <summary>
    /// Information responses
    /// </summary>
    public enum NoxInformationalResponseCodes
    {
        [Description("The server has received the request headers and the client should proceed to send the request body.")]
        Continue = 100,

        [Description("The requester has asked the server to switch protocols.")]
        SwitchingProtocols = 101,

        [Description("This code indicates that the server has received and is processing the request, but no response is available yet.")]
        Processing = 102,
    }
}