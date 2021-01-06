using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Helpers;
using xApi.Data.Interfaces;
using xApi.Data.Json;

namespace xApi.Data
{
    /// <summary>
    /// A collection of Statements can be retrieved by performing a query on the Statement Resource, see Statement Resource for details.
    /// </summary>
    public class StatementsResult : JsonModel, IStatementsResult, IAttachmentByHash
    {
        public StatementsResult() { }

        public StatementsResult(JsonString jsonString) : this(jsonString.ToJToken(), ApiVersion.GetLatest()) { }

        public StatementsResult(JToken jobj, ApiVersion version)
        {
            GuardType(jobj, JTokenType.Object);

            var statements = jobj["statements"];
            if (statements != null)
            {
                Statements = new StatementCollection(statements, version);
            }

            var more = jobj["more"];
            if (more != null)
            {
                More = new Uri(more.Value<string>());
            }
        }

        /// <summary>
        /// List of Statements. 
        /// If the list returned has been limited (due to pagination), 
        /// and there are more results, they will be located at 
        /// the "statements" property within the container located at 
        /// the IRL provided by the "more" property of this Statement 
        /// result Object. Where no matching Statements are found, 
        /// this property will contain an empty array.
        /// </summary>
        public StatementCollection Statements { get; set; }

        /// <summary>
        /// Relative IRL that can be used to fetch more results, including 
        /// the full path and optionally a query string but excluding scheme, 
        /// host, and port. Empty string if there are no more results to fetch.
        /// </summary>
        public Uri More { get; set; }

        public Attachment GetAttachmentByHash(string hash)
        {
            return Statements.GetAttachmentByHash(hash);
        }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = new JObject
            {
                ["statements"] = Statements?.ToJToken(version, format),
                ["more"] = More?.ToString()
            };
            return obj;
        }
    }
}