using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICE.Api.Common.Models;

namespace ICE.Api.Presentation.Models.Request.v1
{
    public class UpdateIdentificationUrlRequest : ActionRequest
    {
        public string ID { get; set; }
        public string URL { get; set; }
    }
}