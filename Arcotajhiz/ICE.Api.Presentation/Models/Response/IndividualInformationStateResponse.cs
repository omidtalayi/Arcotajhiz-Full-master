using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICE.Entities.Enums;

namespace ICE.Api.Presentation.Models.Response
{
    public class IndividualInformationStateResponse
    {
        public string requestID { get; set; }
        public IdentificationStateEnum requestState { get; set; }
        public string reportLink { get; set; }
        public string reportData { get; set; }
        public string emptyReportData { get; set; }
    }
}