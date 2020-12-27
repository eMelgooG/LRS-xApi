using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xApi.Data.Helpers;

namespace xApi.Data.Interfaces
{
    public interface IStatement
    {
        Guid? Id { get; set; }
        Agent Actor { get; set; }
        AttachmentCollection Attachments { get; set; }
        Agent Authority { get; set; }
        Context Context { get; set; }
        IStatementObject Object { get; set; }
        Result Result { get; set; }
        DateTimeOffset? Stored { get; set; }
        DateTimeOffset? Timestamp { get; set; }
        Verb Verb { get; set; }
        ApiVersion Version { get; set; }
        string ToJson();
        void Stamp();
    }
}
