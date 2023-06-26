using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Firm.Models.Request.v1
{
    public class ChangeUsernameRequest
    {
        public string newUsername { get; set; }
        public string password { get; set; }
    }
}