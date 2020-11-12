using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xApi.Data.Exceptions
{
    public class ExperienceDataException : Exception
    {
        public ExperienceDataException() : base() { }

        public ExperienceDataException(string message) : base(message)
        {
        }

        public ExperienceDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
