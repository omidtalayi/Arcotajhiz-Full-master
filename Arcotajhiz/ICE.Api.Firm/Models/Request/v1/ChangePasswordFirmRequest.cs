using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Firm.Models.Request.v1
{
    public class ChangePasswordFirmRequest
    {
        public string UserName { get; set; }
        public string oldPassword { get; set; }
        public string newpassword { get; set; }
        public string repeatnewPassword { get; set; }
    }
}