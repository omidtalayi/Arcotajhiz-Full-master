using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARCO.Api.Presentation.Models.Response
{
    public class ResendCodeIndevidualInformation
    {
        public string requestID { get; set; }
        public int codeExpireTime { get; set; }
        public string responseMessage { get; set; }
        public string reportLink { get; set; }
        public bool resendLocked { get; set; } = false;
    }
}