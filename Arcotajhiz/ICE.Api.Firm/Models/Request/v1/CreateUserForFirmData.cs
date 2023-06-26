using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Firm.Models.Request.v1
{
    public class CreateUserForFirmData
    {
        public string username { get; set; }
        public string password { get; set; }
        public string firmCode { get; set; }
    }
}