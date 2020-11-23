using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Documents
{
    public interface IDocument
    {
        byte[] Content { get; set; }
        string ContentType { get; set; }
        string Checksum { get; set; }
        DateTimeOffset? LastModified { get; set; }
        DateTimeOffset CreateDate { get; set; }
        string Tag { get; set; }
    }
}