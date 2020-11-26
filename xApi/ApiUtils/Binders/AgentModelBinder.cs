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
            var modelName = bindingContext.ModelName;
            var valueProviderResult =
               bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == null)
            {
                return true;
            }
            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            try
            {
                var agent = (Agent)valueProviderResult.RawValue;
                bindingContext.Model = agent;
                return true;
            }
            catch (System.Exception)
            {
                bindingContext.ModelState.AddModelError("Error:", new ArgumentException());
                return false;
            }

            return true;
        }
    }
}