using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Firm.Models.Request.v1
{
    public class AuthenticationInputRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DeviceId { get; set; }
    }
}