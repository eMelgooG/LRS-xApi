using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Exceptions
{
    public class ObjectTypeException : ExperienceDataException
    {
        public ObjectTypeException(string type)
            : base($"'{type}' is not a valid ObjectType.")
        {
        }
    }
}