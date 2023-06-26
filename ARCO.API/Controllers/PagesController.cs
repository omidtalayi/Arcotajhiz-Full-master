using ARCO.Api.Presentation.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ARCO.Entities.Enums;
using ARCO.Entities;
using ARCO.Api.Common.Models;
using ARCO.Api.Common.Enums;
using ARCO.Business.Models;
using ARCO.API.Models.Response;
using System.Web.Http.Cors;
using ARCO.Entities.Models;

namespace ARCO.API.Controllers
{
    [RoutePrefix("api/v1/pages")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PagesController : ApiController
    {
        // GET: api/Pages
        [Route("getpages")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.PAGES))]
        public IHttpActionResult Getpages(int pageNo = 0, int pageSize = 0,int pageType =1)
        {
            var response = new ActionResponse<PagesResponse>();
            var data = new PagesResponse();
            try
            {
                int rowCount = 0;
                data.pages = Business.Engine.Instance.ContentBusiness.GetPages(pageNo, pageSize,ref rowCount,pageType);
                response.Data = data;
                response.Data.rowCount = rowCount;
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

        [Route("getpage")]
        [HttpGet]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.PAGES))]
        // GET: api/Pages/5
        public IHttpActionResult Get(Guid? id)
        {
            var response = new ActionResponse<PagesEntity>();
            try
            {
                int rowCount = 0;
                var page = Business.Engine.Instance.ContentBusiness.GetPage(id);
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

        [Route("pageIns")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.PAGES))]
        // GET: api/Pages/5
        public IHttpActionResult PagesIns(PagesEntity data)
        {
            var response = new ActionResponse<PagesEntity>();
            try
            {
                int rowCount = 0;
                var page = Business.Engine.Instance.ContentBusiness.PagesIns(data.Name,data.PagesType,data.Title,data.Image,data.Description,data.Body,data.Keywords);
                response.State = ResponseStateEnum.SUCCESS;
                response.Data = page;
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

        [Route("deletePage")]
        [HttpPost]
        [JwtAuthentication]
        [APIAuthorize(Roles = nameof(ApiEnum.PAGES))]
        public IHttpActionResult DeletePage(Guid? id)
        {
            var response = new ActionResponse<PagesEntity>();

            try
            {
                var item = Business.Engine.Instance.ContentBusiness.DeletePage(id);
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
    }
}
