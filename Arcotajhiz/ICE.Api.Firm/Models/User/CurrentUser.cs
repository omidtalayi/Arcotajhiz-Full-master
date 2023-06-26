using System;
using System.Collections.Generic;
using ICE.Business.Models;
using ICE.Entities.Enums;

namespace ICE.Api.Firm.Models.User
{
    public class CurrentUser
    {
        public int VCode { get; set; }
        public string UserName { get; set; }
        public DateTime TokenExpireTime { get; set; }
        public string TokenCode { get; set; }
        public string token { get; set; }
        public List<ApiData> apis { get; set; }
    }
}