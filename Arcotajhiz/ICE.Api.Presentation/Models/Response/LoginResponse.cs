using System;

namespace ICE.Api.Presentation.Models.Response
{
    public class LoginResponse
    {
        public string token { get; set; }
        public string userName { get; set; }
        public DateTime tokenExpireDate { get; set; }
    }
}