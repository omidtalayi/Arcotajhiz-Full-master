using ARCO.Api.Common.Class;
using ARCO.Api.Common.Enums;
using ARCO.Api.Common.Models;
using ARCO.Api.Presentation.Filters;
using ARCO.Entities;
using ARCO.Entities.Models;
using ARCO.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ARCO.API.Controllers
{
    public class ResponseWithRowCount<T> {
        public int rowCount { get; set; }
        public T responseData { get; set; }
    }

    [RoutePrefix("api/v1/product")]

    public class ProductController : BaseApiPresentationController
    {
        // GET: Product
        [Route("getproducts")]
        [HttpGet]   
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.PRODUCTS))]
        public IHttpActionResult Getproducts(int pageNo = 0,int pageSize = 0,Guid? categoryId = null,string search = "")
        {
            var response = new ActionResponse<ResponseWithRowCount<List<ProductEntity>>>();

            try
            {
                var rowCount = 0;
                var objProducts = new ResponseWithRowCount<List<ProductEntity>>();
                objProducts.responseData = ARCO.Business.Engine.Instance.ProductBusiness.GetAllProducts(pageNo, pageSize, categoryId, search,null,ref rowCount );
                objProducts.rowCount = rowCount;
                response.Data = objProducts;
                response.State = ResponseStateEnum.SUCCESS;
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                //General.LogError(error: ex, HttpContext.Current.Request);
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
        }
        [Route("getproduct")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.PRODUCTS))]
        public IHttpActionResult Getproduct(Guid? Id = null)
        {
            var response = new ActionResponse<ProductEntity>();

            try
            {

                var objProducts = new ProductEntity();
                objProducts = ARCO.Business.Engine.Instance.ProductBusiness.GetProduct(Id);
                response.Data = objProducts;
                response.State = ResponseStateEnum.SUCCESS;
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
        }
        [Route("getCategories")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.PRODUCTS))]
        public IHttpActionResult GetCategories(int pageNo = 0, int pageSize = 0)
        {
            var response = new ActionResponse<List<CategoryEntity>>();

            try
            {

                List<CategoryEntity> items = new List<CategoryEntity>();
                items = ARCO.Business.Engine.Instance.ProductBusiness.GetCategories(pageNo, pageSize);
                response.Data = items;
                response.State = ResponseStateEnum.SUCCESS;
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                //General.LogError(error: ex, HttpContext.Current.Request);
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
        }
        [Route("getProperties")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.PRODUCTS))]
        public IHttpActionResult GetProperties(int pageNo = 0, int pageSize = 0)
        {
            var response = new ActionResponse<List<PropertyEntity>>();

            try
            {

                List<PropertyEntity> items = new List<PropertyEntity>();
                items = ARCO.Business.Engine.Instance.ProductBusiness.GetProperties(pageNo, pageSize);
                response.Data = items;
                response.State = ResponseStateEnum.SUCCESS;
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                //General.LogError(error: ex, HttpContext.Current.Request);
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
        }
        [Route("addProducts")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.PRODUCTS))]
        public IHttpActionResult AddProduct(ProductEntity product)
        {
            var response = new ActionResponse<ProductEntity>();

            try
            {
                Guid x = new Guid("{00000000-0000-0000-0000-000000000000}");
                if (product.id != null && product.id != x)
                {
                    var productEntity = Business.Engine.Instance.ProductBusiness.UpdateProduct(product);
                    response.Data = productEntity;
                }
                else
                {
                    var productEntity = Business.Engine.Instance.ProductBusiness.AddProduct(product);
                    response.Data = productEntity;
                }
                response.State = ResponseStateEnum.SUCCESS;
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);

            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                //General.LogError(error: ex, HttpContext.Current.Request);
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
        }
        [Route("addCategory")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.PRODUCTS))]
        public IHttpActionResult AddCategory(CategoryEntity category)
        {
            var response = new ActionResponse<CategoryEntity>();

            try
            {
                var categoryEntity = Business.Engine.Instance.ProductBusiness.AddCategory(category);
                response.Data = categoryEntity;
                response.State = ResponseStateEnum.SUCCESS;
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                //General.LogError(error: ex, HttpContext.Current.Request);
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
        }
        [Route("addProperty")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.PRODUCTS))]
        public IHttpActionResult AddProperty(PropertyEntity property)
        {
            var response = new ActionResponse<PropertyEntity>();

            try
            {
                var propertyEntity = Business.Engine.Instance.ProductBusiness.AddProperty(property);
                response.Data = propertyEntity;
                response.State = ResponseStateEnum.SUCCESS;
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                //General.LogError(error: ex, HttpContext.Current.Request);
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
        }
        [Route("deleteProducts")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.PRODUCTS))]
        public IHttpActionResult DeleteProduct(Guid? id)
        {
            var response = new ActionResponse<ProductEntity>();

            try
            {
                var productEntity = Business.Engine.Instance.ProductBusiness.DeleteProduct(id);
                response.Data = productEntity;
                response.State = ResponseStateEnum.SUCCESS;
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                //General.LogError(error: ex, HttpContext.Current.Request);
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
        }

        [Route("deleteCategoy")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.PRODUCTS))]
        public IHttpActionResult DeleteCategory(Guid? id)
        {
            var response = new ActionResponse<CategoryEntity>();

            try
            {
                var item = Business.Engine.Instance.ProductBusiness.DeleteCategory(id);
                response.Data = item;
                response.State = ResponseStateEnum.SUCCESS;
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                //General.LogError(error: ex, HttpContext.Current.Request);
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
        }

        [Route("deleteProperty")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.PRODUCTS))]
        public IHttpActionResult DeleteProperty(Guid? id)
        {
            var response = new ActionResponse<CategoryEntity>();

            try
            {
                var item = Business.Engine.Instance.ProductBusiness.DeleteProperty(id);
                response.Data = null;
                response.State = ResponseStateEnum.SUCCESS;
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
            catch (Exception ex)
            {
                response.State = ResponseStateEnum.FAILED;
                response.Errors.Add(ApiErrorCodeEnum.SERVER_ERROR.ToString());
                //General.LogError(error: ex, HttpContext.Current.Request);
                var objResponse = Request.CreateResponse(HttpStatusCode.OK, response);
                return ResponseMessage(objResponse);
            }
        }
    }

    
}