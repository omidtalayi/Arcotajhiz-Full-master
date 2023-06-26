using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Firm.Models.Request.v1
{
    public class RequestUpdateUserWebhookAndIps
    {
        public long RequestRegistrationVCode { get; set; }
        public string WebHook { get; set; }
        public string Ips { get; set; }
    }
}