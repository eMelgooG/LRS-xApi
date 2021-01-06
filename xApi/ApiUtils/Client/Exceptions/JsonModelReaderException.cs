using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.ApiUtils.Client.Exceptions
{
    public class JsonModelReaderException : Exception
    {
        public JsonModelReaderException(string message) : base(message)
        {
        }

        public JsonModelReaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}