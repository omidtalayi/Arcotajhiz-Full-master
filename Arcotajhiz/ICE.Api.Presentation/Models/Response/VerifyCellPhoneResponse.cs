using ICE.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Presentation.Models.Response
{
    public class VerifyCellPhoneResponse
    {
        public VerifyCellphoneStateApiEnum verifyState { get; set; }
        public string reportProcessingLink { get; set; }
        public string verifyStateMessage { get; set; }
    }
}