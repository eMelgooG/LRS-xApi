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
    public class StatementRef : StatementObjectBase, IStatementObject, IStatementRef
    {
        public StatementRef() { }
        public StatementRef(JsonString jsonString) : this(jsonString.ToJToken()) { }
        public StatementRef(JToken jobj) : this(jobj, ApiVersion.GetLatest()) { }
        public StatementRef(JToken jobj, ApiVersion version)
        {
            GuardType(jobj, JTokenType.Object);

            var id = jobj["id"];
            if (id != null)
            {
                GuardType(id, JTokenType.String);
                Id = ParseGuid(id);
            }
        }

        protected override ObjectType OBJECT_TYPE => ObjectType.StatementRef;

        /// <summary>
        /// The UUID of a Statement.
        /// </summary>
        public Guid Id { get; set; }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = base.ToJToken(version, format);

            jobj["id"] = Id.ToString();

            return jobj;
        }

        public override bool Equals(object obj)
        {
            var @ref = obj as StatementRef;
            return @ref != null &&
                   base.Equals(obj) &&
                   Id.Equals(@ref.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<Guid>.Default.GetHashCode(Id);
        }

        public static implicit operator StatementRef(JObject jobj)
        {
            return new StatementRef(jobj);
        }

        public static bool operator ==(StatementRef ref1, StatementRef ref2)
        {
            return EqualityComparer<StatementRef>.Default.Equals(ref1, ref2);
        }

        public static bool operator !=(StatementRef ref1, StatementRef ref2)
        {
            return !(ref1 == ref2);
        }
    }
}