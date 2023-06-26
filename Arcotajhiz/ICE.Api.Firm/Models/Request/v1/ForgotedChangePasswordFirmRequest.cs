using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Firm.Models.Request.v1
{
    public class ForgotedChangePasswordFirmRequest
    {
        public string id { get; set; }
        public string cellphone { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
    }
}