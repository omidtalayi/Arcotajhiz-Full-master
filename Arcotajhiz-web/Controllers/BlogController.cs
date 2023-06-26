using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Arcotajhiz.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Index(int pageNo = 1,int pageSize = 20)
        {
            int rowCount = 0;
            var items = ARCO.Business.Engine.Instance.ContentBusiness.GetPages(pageNo,pageSize,ref rowCount,1);
            ViewData["items"] = items;
            return View();
        }
        public ActionResult BlogDetails(Guid? id)
        {
            var item = ARCO.Business.Engine.Instance.ContentBusiness.GetPage(id);
            ViewData["item"] = item;
            return View();
        }
    }
}