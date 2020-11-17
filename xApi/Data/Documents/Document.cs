using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Documents
{
    public abstract class Document
    {
        /// <summary>
        /// Gets or sets the opaque quoted string.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Last Modified
        /// </summary>
        public DateTimeOffset? LastModified { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }
    }
}