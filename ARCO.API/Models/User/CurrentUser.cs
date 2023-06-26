using ARCO.Business.Models;
using System;
using System.Collections.Generic;

namespace ARCO.Api.Presentation.Models.User
{
    public class CurrentUser
    {
        public long VCode { get; set; }
        public string UserName { get; set; }
        public DateTime TokenExpireTime { get; set; }
        public string TokenCode { get; set; }
        public string token { get; set; }
        public List<ApiData> apis { get; set; }
    }
}