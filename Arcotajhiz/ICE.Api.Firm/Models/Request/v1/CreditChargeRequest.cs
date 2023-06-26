using ICE.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICE.Api.Firm.Models.Request.v1
{
    public class CreditChargeRequest
    {
        public decimal Amount { get; set; }
        public BankEnum BankVCode { get; set; } = BankEnum.SAMAN;
        public PortalPaymentTypeEnum PortalPaymentTypeVCode { get; set; } = PortalPaymentTypeEnum.CREDIT;
        public long RequestRegistrationVCode { get; set; }
    }
}