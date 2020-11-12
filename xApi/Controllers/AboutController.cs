﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using xApi.Data;
using xApi.Filters;

namespace xApi.Controllers
{
    [AllowAnonymous]
    [RequiredVersionHeader]
    [Route("xapi/about")]
    public class AboutController : ApiController
    {
        [HttpGet]   
        public IHttpActionResult GetAbout()
        {
   
            var about = About.Instance;
            return Ok(about);
        }

    }
}