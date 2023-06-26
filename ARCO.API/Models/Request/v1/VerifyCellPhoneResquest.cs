using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARCO.Api.Presentation.Models.Request.v1
{
    public class VerifyCellPhoneResquest
    {
        public string requestID { get; set; }
        public int verifyCode { get; set; }
    }
}