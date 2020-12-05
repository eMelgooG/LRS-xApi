using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        //Merge documents if both have app/json
        public void MergeDocument(Document postedDoc)
        {
            //deserialize objects
            Dictionary<String, Object> doc = Helpers.Helpers.ParseJsonByteArrayToDictionary(this.Content);
            Dictionary<String, Object> posted = Helpers.Helpers.ParseJsonByteArrayToDictionary(postedDoc.Content);
            foreach (KeyValuePair<string, object> entry in posted)
            {
                doc[entry.Key] = entry.Value;
                }
            //serialize the object to JSON
            var json = JsonConvert.SerializeObject(doc);
            //to byte array
            var bytes = Encoding.UTF8.GetBytes(json);
            //update the document with the new values
            this.Content = bytes;
            this.LastModified = DateTimeOffset.UtcNow;
            Checksum = this.GenerateChecksum();
        }
    }
}