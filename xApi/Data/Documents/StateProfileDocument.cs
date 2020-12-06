using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Documents
{
    public class StateProfileDocument : Document
    {
        public StateProfileDocument(byte[] body, string contentType) : base(body, contentType)
        {
        }
        public StateProfileDocument()
        {
        }
        public int Id { get; set; }
        public string StateId { get; set; }
        public Iri Activity { get; set; }
        public Agent Agent { get; set; }
        public Guid? Registration { get; set; }
    }
}