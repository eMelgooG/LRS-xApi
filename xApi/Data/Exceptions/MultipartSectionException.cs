using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Exceptions
{
    public class MultipartSectionException : ExperienceDataException
    {
        public MultipartSectionException(string message)
            : base(message)
        {
        }
    }
}