using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Presentation.Models.Response
{
    public class SetIndividualInformationResponse
    {
        public string requestID { get; set; }
        public int codeExpireTime { get; set; }
        public string responseMessage { get; set; }
        public string reportLink { get; set; }
    }
}