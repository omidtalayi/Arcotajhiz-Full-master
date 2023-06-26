using ARCO.Business.Models;
using ARCO.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARCO.API.Models.Response
{
    public class PagesResponse
    {
        public PagesResponse() {
            pages = new List<PagesEntity>();
        }
        public int rowCount { get; set; }
        public List<PagesEntity> pages { get; set; }
    }
}