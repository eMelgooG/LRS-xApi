using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Domain
{
    public class StatementEntity
    {
       public Guid? Id { get; set; }
        public int VerbId { get; set; }
        public byte ObjectType { get; set; }
        public int ObjectId { get; set; }
        public string FullStatement { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public DateTimeOffset? Stored { get; set; }
        public string Version { get; set; }
        public bool IsVoided { get; set; }
        public bool IsVoiding { get; set; }
    }
}