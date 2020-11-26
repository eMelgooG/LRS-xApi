using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Json.Exceptions;

namespace xApi.Data.Exceptions
{
    public class InvalidDateTimeOffsetException : JsonTokenModelException
    {
        public InvalidDateTimeOffsetException(JToken token, string date)
            : base(token, $"'{date}' does not allow an offset of -00:00, -0000, -00")
        {
        }
    }
}