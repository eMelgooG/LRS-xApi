using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Exceptions;

namespace xApi.Data.Json.Exceptions
{
    public class JsonModelException : ExperienceDataException
    {
        public JsonModelException() : base()
        {
        }

        public JsonModelException(string message)
            : base(message)
        {
        }

        public JsonModelException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}