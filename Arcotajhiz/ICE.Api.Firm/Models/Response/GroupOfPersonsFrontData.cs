﻿using System;
using ICE.Business.Models;
using ICE.Entities.Enums;

namespace ICE.Api.Firm.Models.Response
{
    public class GroupOfPersonsFrontData
    {
        public long VCode { get; set; }
        public string Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string Cellphone { get; set; }
        public string NationalCode { get; set; }
        public string CompanyNationalID { get; set; }
        public UserPaymentTypeEnum UserPaymentTypeEnum { get; set; }
        public string UserPaymentTypeName { get; set; }
        public GroupOfPersonsStateEnum GroupOfPersonsState { get; set; }
        public string ReportLink { get; set; }
        public string ResponseMessage { get; set; }
        public string GroupOfPersonsStateName { get; set; }
        public string UserPaymentTypeEnumName { get; set; }
    }
}