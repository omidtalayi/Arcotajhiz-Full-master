using System;

namespace ICE.Api.Firm.Models.Response
{
    public class IdentificationVerifyResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public int expire { get; set; }
    }
}