using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Documents
{
    public class AgentProfileDocument : Document
    {
        public AgentProfileDocument(byte[] body, string contentType) : base(body, contentType)
        {
        }
        public AgentProfileDocument() { }

        public int Id { get; set; }
        public Agent Agent { get; set; }

        public string ProfileId { get; set; }
    }
}