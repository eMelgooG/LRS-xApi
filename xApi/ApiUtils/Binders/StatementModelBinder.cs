using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using xApi.Data;

namespace xApi.ApiUtils.Binders
{
    public class StatementModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(Statement))
            {
                return false;
            }
            var modelName = bindingContext.ModelName;
            var body = actionContext.Request.Content.ReadAsStreamAsync();
            var headers = actionContext.Request.Headers;
            try
            {

            }

            return false;

    
        }
    }
}