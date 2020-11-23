using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Exceptions
{
    public class MboxFormatException : ExperienceDataException
    {
        public MboxFormatException(string message) : base(message)
        {
        }

        public MboxFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}