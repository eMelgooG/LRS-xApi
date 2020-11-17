using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Documents
{
    public class ActivityProfileDocument : Document
    {
        public Uri ActivityId { get; set; }

        public string ProfileId { get; set; }

        public Guid? Registration { get; set; }
    }
}