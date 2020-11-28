using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using xApi.Data;

namespace xApi.ApiUtils.Binders
{
    public class AgentModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
             if (bindingContext.ModelType != typeof(Iri))
            {
                return false;
            }
            var modelName = bindingContext.ModelName;
            var valueProviderResult =
               bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == null)
            {
                return false;
            }
            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
            var val = valueProviderResult.RawValue as string;
            try
            {
                var agent = new Agent(valueProviderResult.RawValue as string);
                bindingContext.Model = agent;
                return true;
            }
            catch (System.Exception ex)
            {
                bindingContext.ModelState.AddModelError(modelName,ex);
                return false;
            }

            return false;
        }
    }
}