using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Arcotajhiz.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var rowC = 0;
            var mainPageProducts = ARCO.Business.Engine.Instance.ProductBusiness.GetAllProducts(pageNo: 1,pageSize: 50,rowC: ref rowC);
            if (mainPageProducts != null && mainPageProducts.Any())
            {
                mainPageProducts.Sort((a, b) => b.createDate.CompareTo(a.createDate));
            }
            ViewData["mainPageProducts"] = mainPageProducts;

            int rowCount = 0;
            var mainPages = ARCO.Business.Engine.Instance.ContentBusiness.GetPages(1, 8,ref rowCount,2);
            if (mainPages != null && mainPages.Any())
            {
                mainPages.Sort((a, b) => b.EntryDate.CompareTo(a.EntryDate));
            }
            ViewData["mainPages"] = mainPages;

            int rowCountArticle = 0;
            var mainArticle = ARCO.Business.Engine.Instance.ContentBusiness.GetPages(1, 2, ref rowCountArticle, 1);
            if (mainArticle != null && mainArticle.Any())
            {
                mainArticle.Sort((a, b) => b.EntryDate.CompareTo(a.EntryDate));
            }
            ViewData["mainArticle"] = mainArticle;
            return View();
        }

     
    }
}