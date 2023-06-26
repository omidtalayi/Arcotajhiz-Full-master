using System;

namespace ICE.Api.Firm.Models.Response
{
    public class ResendVerifyCodeResponse
    {
        public string cellphone { get; set; }
        public string message { get; set; }
        public string firmCode { get; set; }
        public DateTime tokenExpireDate { get; set; }
    }
}