using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Firm.Models.Request.v1
{
    public class ChangeEmailRequest
    {
        public string newEmail { get; set; }
        public string password { get; set; }
    }
}