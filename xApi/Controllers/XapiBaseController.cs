using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using xApi.Data.Helpers;
using xApi.Filters;

namespace xApi.Controllers
{
    //[RequiredVersionHeader]
    //[Authorize]
    public class XapiBaseController : ApiController
    {


        public ApiVersion APIVersion
        {
            get
            {
               return Request.Headers.GetValues(ApiHeaders.XExperienceApiVersion).Single();
            }
        }
    }
   
}
