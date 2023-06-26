using ICE.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ICE.Api.Firm.Models.Request.v1
{
    public class GetUserCreditsRequestBody
    {
        public int pageSize { get; set; }
        public int pageNo { get; set; }
    }
}