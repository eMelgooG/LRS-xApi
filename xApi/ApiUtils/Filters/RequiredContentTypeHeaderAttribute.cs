using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace xApi.ApiUtils.Filters
{
    public class RequiredContentTypeHeaderAttribute : ActionFilterAttribute
    {
        public static string CONTENT_TYPE = "Content-Type";
        public static string APP_JSON = "application/json";
        public static string MULT_MIXED = "multipart/mixed";

        public override void OnActionExecuting(HttpActionContext context)
        {

            try
            {
                //Well-known content-specific headers are grouped under "content headers". 
                //It's just for convenience. There are headers like content-type, content-length, and so on.
                if (!context.Request.Content.Headers.Contains(CONTENT_TYPE))
                {
                    throw new Exception($"Missing '{CONTENT_TYPE}' header.");
                }
                var contentType = context.Request.Content.Headers.GetValues(CONTENT_TYPE).First();
                if (!contentType.Equals(APP_JSON, StringComparison.CurrentCultureIgnoreCase) && !contentType.Equals(MULT_MIXED, StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new Exception($"Invalid content type value '{contentType}'.");
                }
            }
            catch (Exception ex)
            {
                var response = context.Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, ex.Message + $" Valid Content-Type values are: '{APP_JSON}' and '{MULT_MIXED}'.");
                context.Response = response;
            }


        }
    }
}