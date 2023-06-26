using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Firm.Models.Request.v1
{
    public class FirmUserData
    {
        public string companyName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string cellphone { get; set; }
        public string tel { get; set; }
        public string postalCode { get; set; }
        public string registeredIdentificationNo { get; set; }
        public string registrationNo { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public int province { get; set; }
        public int city { get; set; }
        public string letterFileName { get; set; }
        public string officialPaperFileName { get; set; }
        public int registerDate { get; set; }
        public string presenterCode { get; set; }
    }
}