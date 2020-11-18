using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Documents
{
    public class ActivityProfileDocument : Document
    {
        public ActivityProfileDocument(byte[] body, string contentType) : base(body, contentType)
        {
        }
        public ActivityProfileDocument() { }
        public Uri ActivityId { get; set; }

        public string ProfileId { get; set; }

        public Guid? Registration { get; set; }
    }
}