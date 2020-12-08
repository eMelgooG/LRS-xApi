using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Exceptions
{
    public class InteractionTypeException : ExperienceDataException
    {
        public InteractionTypeException(string type)
            : base($"'{type}' is not a valid InteractionType.")
        {
        }
    }
}