using ICE.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Firm.Models.Request.v1
{
    public class FirmRequestChargeRequest
    {
        public long RequestVCode { get; set; }
        public string FirmName { get; set; }
        public string ContactPointName { get; set; }
        public string ContactPointFamilyName { get; set; }
        public string RegisteredIdentificationNo { get; set; }
        public string RegistrationNo { get; set; }
        public string Tel { get; set; }
        public string FirmPostalCode { get; set; }
        public long cityVCode { get; set; }
        public string Address { get; set; }
        public string officialPaperFileName { get; set; }
        public string idCardFileName { get; set; }
    }
}