using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Firm.Models.Response
{
    public class CheckIndividualReportExistenceResponse
    {
        public bool ReportIsGeneratedPast24Hours { get; set; }
    }
}