using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Exceptions
{
    public class DurationFormatException : ExperienceDataException
    {
        public DurationFormatException(string message) : base(message)
        {
        }

        public DurationFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}