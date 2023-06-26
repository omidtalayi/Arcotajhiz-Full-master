using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Firm.Models.Request.v1
{
    public class PanelCodeConfirmationRequest
    {
        public string reportID { get; set; }
        public long code { get; set; }
    }
}