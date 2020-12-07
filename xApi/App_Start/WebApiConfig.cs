using FluentValidation.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
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

            //formatters. Note: WEB API uses the first formatters in the pipeline. So if you have a custom formatter put it first.
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            

            // Web API routes
            config.MapHttpAttributeRoutes();
            FluentValidationModelValidatorProvider.Configure(config);


        }
    }
}
