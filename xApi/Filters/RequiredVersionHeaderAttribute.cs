using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using xApi.Data.Helpers;

namespace xApi.Filters
{
    public class RequiredVersionHeaderAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            var supported = ApiVersion.GetSupported();
            try
            {
                if (!context.Request.Headers.Contains(ApiHeaders.XExperienceApiVersion))
                {
                    throw new Exception($"Missing '{ApiHeaders.XExperienceApiVersion}' header.");
                }

                IEnumerable<string> headerValues = context.Request.Headers.GetValues(ApiHeaders.XExperienceApiVersion);
                string requestVersion = headerValues.FirstOrDefault();
                if (string.IsNullOrEmpty(requestVersion))
                {
                    throw new Exception($"'{ApiHeaders.XExperienceApiVersion}' header or it's null or empty.");
                }

                try
                {
                    ApiVersion version = requestVersion;
                }
                catch (Exception)
                {
                    throw new Exception($"'{ApiHeaders.XExperienceApiVersion}' header is '{requestVersion}' which is not supported.");
                }

            }
            catch (Exception ex)
            {
                var response = context.Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, ex.Message + " Supported Versions are: " + string.Join(", ", supported.Select(x => x.Key)));
                context.Response = response;
                context.Response.Headers.Add(ApiHeaders.XExperienceApiVersion, ApiVersion.GetLatest().ToString());
   
            }
        }
    }
    }

