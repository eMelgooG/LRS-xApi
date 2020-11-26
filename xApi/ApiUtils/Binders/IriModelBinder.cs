using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using xApi.Data;

namespace xApi.ApiUtils.Binders
{
    public class IriModelBinder : IModelBinder
    {
        public IriModelBinder() { }
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(Iri))
            {
                return false;
            }
            var modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == null)
            {
                return false;
            }
            //set the string you receive it inside the modelstate so you can retrieve it in the controller to display error (xoxoxo : ) 
            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
            var val = valueProviderResult.RawValue as string;

            if (Iri.TryParse(val, out Iri iri)) {
                bindingContext.Model = iri;
                return true;
            } else {
                bindingContext.ModelState.AddModelError(
          bindingContext.ModelName, $"Please provide a valid iri: '{val}' is an invalid IRI.");
                return false;
            }
            return false;
        }

    }
    }

