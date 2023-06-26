using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ICE.Api.Firm.Models.Credit
{
    [DataContract]
    [Serializable]
    public class SepTxnData
    {
        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public string TerminalId { get; set; }

        [DataMember]
        public string RedirectUrl { get; set; }

        [DataMember]
        public string ResNum { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string Amounts { get; set; }

        [DataMember]
        public long CellNumber { get; set; }

        [DataMember]
        public int MultiSettle { get; set; }
    }
}