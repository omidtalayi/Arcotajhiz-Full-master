using ICE.Api.Common.Models;
using ICE.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ICE.Api.Presentation.Models.Response
{
    public class ApiIsDisabledResponse : ActionRequest
    {
        public bool IsDisabled { get; set; }
    }
}