using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Firm.Models.Request.v1
{
    public class CancelIdentificationFirmRequest
    {
        public string reportID { get; set; }
        public long vcode { get; set; }
    }
}