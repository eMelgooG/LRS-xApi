using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using xApi.ApiUtils.Client;
using xApi.ApiUtils.Client.Exceptions;
using xApi.Data;

namespace xApi.ApiUtils.Binders
{
    public class StatementModelBinder : IModelBinder
    {
        public StatementModelBinder() { }
        public  bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(Statement))
            {
                return false;
            }
            var modelName = bindingContext.ModelName;
            var body = actionContext.Request.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
            var headers = actionContext.Request.Content.Headers;
            try
            {
                var jsonModelReader = new JsonModelReader(headers, body);
                Statement statement =  jsonModelReader.ReadAs<Statement>().GetAwaiter().GetResult();
                if (statement != null)
                {
                    bindingContext.Model = statement;
                    return true;
                }
            } catch (JsonModelReaderException ex)
            {
                bindingContext.ModelState.AddModelError(modelName, ex);
                return false;
            }

            return false;

    
        }
    }
}