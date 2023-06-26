using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Arcotajhiz.Filters
{
    public class Layout : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction &&
                !filterContext.HttpContext.Request.IsAjaxRequest() &&
                filterContext.Result is ViewResult)
            {
                var identity = new GenericIdentity("", "");
                var principal = new GenericPrincipal(identity, new string[] { });
                var viewBag = filterContext.Controller.ViewBag;
                viewBag.Menus = ARCO.Business.Engine.Instance.ProductBusiness.GetCategories();
            }
        }
    }
}