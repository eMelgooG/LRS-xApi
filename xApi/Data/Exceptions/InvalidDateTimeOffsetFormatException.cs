using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Json.Exceptions;

namespace xApi.Data.Exceptions
{
    public class InvalidDateTimeOffsetFormatException : JsonTokenModelException
    {
        public InvalidDateTimeOffsetFormatException(JToken token, string date)
            : base(token, $"'{date}' is not a well formed DateTimeOffset.")
        {
        }
    }
}