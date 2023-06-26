using System;

namespace ICE.Api.Firm.Models.Response
{
    public class VerifyCellPhoneResponse
    {
        public string cellphone { get; set; }
        public string message { get; set; }
        public string username { get; set; }
        public string token { get; set; }
        public DateTime tokenExpireDate { get; set; }
    }
}