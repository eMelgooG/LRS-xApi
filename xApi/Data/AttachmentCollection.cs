using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Helpers;
using xApi.Data.Json;

namespace xApi.Data
{
    /// <summary>
    /// A collection of <see cref="Attachment"/> objects.
    /// </summary>
    //[JsonConverter(typeof(AttachmentCollectionConverter))]
    public class AttachmentCollection : JsonModel<JArray>, ICollection<Attachment>
    {
        private readonly HashSet<Attachment> Attachments = new HashSet<Attachment>();

        public int Count => Attachments.Count;

        public bool IsReadOnly => ((ICollection<Attachment>)Attachments).IsReadOnly;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentCollection"/> class.
        /// </summary>
        public AttachmentCollection() { }
        public AttachmentCollection(JsonString jsonString) : this(jsonString.ToJToken()) { }
        public AttachmentCollection(JToken jarr) : this(jarr, ApiVersion.GetLatest()) { }
        public AttachmentCollection(JToken jarr, ApiVersion version)
        {
            GuardType(jarr, JTokenType.Array);

            foreach (var item in jarr)
            {
                Add(new Attachment(item, version));
            }
        }


        public Attachment GetAttachment(string sha2)
        {
            if (TryGetAttachment(sha2, out Attachment attachment))
            {
                return attachment;
            }

            return null;
        }

        public bool TryGetAttachment(string sha2, out Attachment attachment)
        {
            attachment = Attachments.FirstOrDefault(x => x.SHA2 == sha2);
            return attachment != null;
        }

        public void Add(Attachment item)
        {
            // TODO: Many it should not fail silently
            if (!Contains(item))
            {
                Attachments.Add(item);
            }
        }

        public void Clear()
        {
            Attachments.Clear();
        }

        public bool Contains(Attachment item)
        {
            // TODO: Should use a custom implementation of Equals and GetHashCode
            return Attachments.Any(x => x.SHA2 == item.SHA2);
        }

        public void CopyTo(Attachment[] array, int arrayIndex)
        {
            Attachments.CopyTo(array, arrayIndex);
        }

        public bool Remove(Attachment item)
        {
            return Attachments.Remove(item);
        }

        public IEnumerator<Attachment> GetEnumerator()
        {
            return ((ICollection<Attachment>)Attachments).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<Attachment>)Attachments).GetEnumerator();
        }

        public override JArray ToJToken(ApiVersion version, ResultFormat format)
        {
            if (Attachments.Count == 0)
            {
                return null;
            }

            var jArray = new JArray();

            foreach (var attachment in Attachments)
            {
                jArray.Add(attachment.ToJToken(version, format));
            }

            return jArray;
        }
    }
}