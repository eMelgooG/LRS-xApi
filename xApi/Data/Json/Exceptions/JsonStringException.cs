using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Exceptions;

namespace xApi.Data.Json.Exceptions
{
    public class JsonStringException : ExperienceDataException
    {
        public JsonStringException(string message) : base(message)
        {
        }

        public JsonStringException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}