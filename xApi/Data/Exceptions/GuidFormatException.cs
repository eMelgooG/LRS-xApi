using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Json.Exceptions;

namespace xApi.Data.Exceptions
{
    public class GuidFormatException : JsonTokenModelException
    {
        public GuidFormatException(JToken token, string guid)
            : base(token, $"'{guid}' is not a valid UUID.")
        {
        }
    }
}