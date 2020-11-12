using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using xApi.BasicAuthentication.Filters;

namespace xApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.SuppressHostPrincipal();
            config.Filters.Add(new IdentityBasicAuthenticationAttribute());
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Formatters.Remove(config.Formatters.XmlFormatter);

        }
    }
}
