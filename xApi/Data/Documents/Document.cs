using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace xApi.Data.Documents
{
    public class Document : IDocument
    {

        public Document() { }

        public Document(byte[] body, string contentType)
        {
            Content = body;
            ContentType = contentType;
            LastModified = DateTimeOffset.UtcNow;
            CreateDate = DateTimeOffset.UtcNow;
            Checksum = GenerateChecksum();
        }

        /// <summary>
        /// Gets or sets the opaque quoted string.
        /// </summary>
        /// 
        public string Tag { get; set; }

        /// <summary>
        /// UTC Date when the document was last modified
        /// </summary>
        public DateTimeOffset? LastModified { get; set; }

        /// <summary>
        /// UTC Date when the document was created
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }

        /// <summary>
        /// Representation of the Content-Type header received
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// The byte array of the document content
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// MD5 Checksum
        /// </summary>
        public string Checksum { get; set; }
        
        //Methods
        private string GenerateChecksum()
        {
            if (Content == null)
            {
                throw new NullReferenceException("Content is null or empty");
            }

            using (var sha1 = SHA1.Create())
            {
                byte[] checksum = sha1.ComputeHash(Content);
                return BitConverter.ToString(checksum).Replace("-", string.Empty).ToLower();
            }
        }

        // Factories:
        public void UpdateDocument(byte[] content, string contentType)
        {
            this.Content = content;
            this.ContentType = contentType;
            this.LastModified = DateTimeOffset.UtcNow;
            Checksum = this.GenerateChecksum();
        }
    }
}