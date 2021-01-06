using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Helpers;
using xApi.Data.Interfaces;
using xApi.Data.Json;

namespace xApi.Data
{
    /// <summary>
    /// A collection of <see cref="Statement"/>s with <see cref="Attachment"/>s
    /// </summary>
    public class StatementCollection : JsonModel, ICollection<Statement>, IAttachmentByHash
    {
        private readonly ICollection<Statement> Statements = new HashSet<Statement>();

        public StatementCollection() { }
        public StatementCollection(IEnumerable<Statement> statements)
        {
            Statements = new HashSet<Statement>(statements);
        }
        public StatementCollection(JsonString jsonString) : this(jsonString.ToJToken(), ApiVersion.GetLatest()) { }
        public StatementCollection(JToken jobj, ApiVersion version)
        {
            GuardType(jobj, JTokenType.Array, JTokenType.Object);

            if (jobj.Type == JTokenType.Array)
            {
                foreach (var item in jobj)
                {
                    Add(new Statement(item, version));
                }
            }
            else if (jobj.Type == JTokenType.Object)
            {
                Add(new Statement(jobj, version));
            }
        }

        public int Count => Statements.Count;

        public bool IsReadOnly => Statements.IsReadOnly;

        public void Add(Statement item)
        {
            Statements.Add(item);
        }

        public void Clear()
        {
            Statements.Clear();
        }

        public bool Contains(Statement item)
        {
            return Statements.Contains(item);
        }

        public void CopyTo(Statement[] array, int arrayIndex)
        {
            Statements.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Find attachment by hash on a statement in the collection.
        /// </summary>
        public Attachment GetAttachmentByHash(string hash)
        {
            foreach (var statement in Statements)
            {
                var attachment = statement.GetAttachmentByHash(hash);
                if (attachment != null)
                {
                    return attachment;
                }
            }

            return null;
        }

        public IEnumerator<Statement> GetEnumerator()
        {
            return Statements.GetEnumerator();
        }

        public bool Remove(Statement item)
        {
            return Statements.Remove(item);
        }

        /// <inheritdoc/>
        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var jarr = new JArray();
            foreach (var stmt in Statements)
            {
                jarr.Add(stmt.ToJToken(version, format));
            }
            return jarr;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Statements.GetEnumerator();
        }
    }
}