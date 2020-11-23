using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data
{
    public class Activity
    {
        public Activity() { }
        public Uri ActivityId { get; set; }
        public ActivityDefinition Definition { get; set; }

    }
}