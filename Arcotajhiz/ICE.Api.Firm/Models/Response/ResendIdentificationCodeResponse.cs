using System;

namespace ICE.Api.Firm.Models.Response
{
    public class ResendIdentificationCodeResponse
    {
        public int resendWait { get; set; }
        public bool resendLocked { get; set; }
        public string message { get; set; }
        public bool hasError { get; set; }
        public string status { get; set; }
    }
}