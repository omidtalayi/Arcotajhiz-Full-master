using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Presentation.Models.Request.v1
{
    public class AuthenticationInputReqeust
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool NewToken { get; set; } = false;
    }
}