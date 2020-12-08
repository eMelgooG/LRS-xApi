using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Interfaces
{
    public interface IActivity
    {
        Iri Id { get; set; }
        ActivityDefinition Definition { get; set; }
    }
}