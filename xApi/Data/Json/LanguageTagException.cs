using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Json.Exceptions;

namespace xApi.Data.Json
{
    public class LanguageTagException : JsonTokenModelException
    {
        public LanguageTagException(JToken token, string cultureName)
            : base(token, $"'{cultureName}' is not a valid RFC5646 Language Tag.")
        {
        }
    }
}