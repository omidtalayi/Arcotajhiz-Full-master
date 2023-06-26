using ARCO.Entities;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Arcotajhiz.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(int pageNo = 1)
        {
            var category = new CategoryEntity();
            var rowC = 0;
            var products = ARCO.Business.Engine.Instance.ProductBusiness.GetAllProducts(pageNo: pageNo, pageSize: 9,rowC : ref rowC).ToList();
            if (products != null && products.Any())
            {
                var productpropsObj = new List<ProductPropertyEntity>();
                var productCategories = new List<CategoryEntity>();
                var productProperties = products.Where(p => p.productProperties != null).ToList().Select(z => z.productProperties).ToList();
                foreach (var item in productProperties)
                {
                    productpropsObj.AddRange(item);
                };
                productpropsObj = productpropsObj.GroupBy(x=> x.value).Select(x=>x.FirstOrDefault()).ToList();
                var test = products.Where(t => t.category != null).Select(x => x.category).ToList();
                productCategories.AddRange(test.GroupBy(x => x.id).Select(x => x.FirstOrDefault()).ToList());
                //var groupedProperies= productpropsObj.GroupBy(g => new Tuple<Guid?, string>(g.Property.id, g.Property.name)).ToList();
                //var groupedProperies = productpropsObj.GroupBy(g => g.Property.name).ToList();
                ViewData["productprops"] = productpropsObj;
                ViewData["productCategories"] = productCategories;
            }
      
            ViewData["products"] = products;
            ViewData["category"] = category;
            
            return View("Index");
        }
        public ActionResult ProductDetails(Guid? id=null,string title = "")
        {
            var product = ARCO.Business.Engine.Instance.ProductBusiness.GetProduct(id);
            ViewData["product"] = product;
            return View();
        }
        public ActionResult Category(Guid? id, string title = "",int pageNo = 1)
        {
            var category = ARCO.Business.Engine.Instance.ProductBusiness.GetCategory(id);
            var rowC =0;
            var products = ARCO.Business.Engine.Instance.ProductBusiness.GetAllProducts(pageNo, 9, id,rowC: ref rowC);
            var jsonProperties = "";
            if (products != null && products.Any())
            {
                var productpropsObj = new List<ProductPropertyEntity>();
                var productCategories = new List<CategoryEntity>();
                var productProperties = products.Where(p => p.productProperties != null).ToList().Select(z => z.productProperties).ToList();
                foreach (var item in productProperties)
                {
                    productpropsObj.AddRange(item);
                };
                productpropsObj = productpropsObj.GroupBy(x => x.value).Select(x => x.FirstOrDefault()).ToList();
                var test = products.Where(t => t.category != null).Select(x => x.category).ToList();
                productCategories.AddRange(test.GroupBy(x => x.id).Select(x => x.FirstOrDefault()).ToList());
                //var groupedProperies= productpropsObj.GroupBy(g => new Tuple<Guid?, string>(g.Property.id, g.Property.name)).ToList();
                //var groupedProperies = productpropsObj.GroupBy(g => g.Property.name).ToList();
                ViewData["productprops"] = productpropsObj;
                ViewData["productCategories"] = productCategories;
            }
            ViewData["products"] = products;
            ViewData["category"] = category;
            return View("Index");
        }

        [HttpGet]
        public ActionResult SearchProducts(string keyword = "",string props = null,int pageNo = 1,Guid? categoryId = null) {
            List<Guid?> guids = new List<Guid?>();
            if (!String.IsNullOrEmpty(props))
            {
                foreach (var item in props.Split(','))
                {
                    guids.Add(new Guid(item));
                }
            }
            var rowC = 0;
            var products = ARCO.Business.Engine.Instance.ProductBusiness.GetAllProducts(pageNo, 9, categoryId, keyword, guids,rowC : ref rowC).ToList();
            ViewData["products"] = products;
            return PartialView("_Products");
        }
    }
}