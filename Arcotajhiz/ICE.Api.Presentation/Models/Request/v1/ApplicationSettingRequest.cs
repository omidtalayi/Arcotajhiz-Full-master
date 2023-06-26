using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ICE.Api.Presentation.Models.Request.v1
{
    public class ApplicationSettingRequest
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}