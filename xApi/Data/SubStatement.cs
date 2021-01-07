using Newtonsoft.Json;
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
    // NOTE: A SubStatement MUST NOT have the "id", "stored", "version" or "authority" properties.
    /// <summary>
    /// A SubStatement is like a StatementRef in that it is included as part of a containing Statement, but unlike a StatementRef, it does not represent an event that has occurred. 
    /// It can be used to describe, for example, a predication of a potential future Statement or the behavior a teacher looked for when evaluating a student (without representing the student actually doing that behavior)
    /// SubStatement <see cref="https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Data.md#substatements"/>
    /// </summary>

    [JsonObject]
    public class SubStatement : StatementBase, IStatementObject, ISubStatement
    {
        public ObjectType ObjectType => ObjectType.SubStatement;

        public SubStatement() : base() { }
        public SubStatement(JsonString jsonString) : this(jsonString.ToJToken()) { }
        public SubStatement(JToken jobj) : this(jobj, ApiVersion.GetLatest()) { }
        public SubStatement(JToken subStatement, ApiVersion version) : base(subStatement, version)
        {
            GuardType(subStatement, JTokenType.Object);

            var @object = subStatement["object"];
            if (@object != null)
            {
                GuardType(@object, JTokenType.Object);
                var objectType = @object["objectType"];
                if (objectType != null)
                {
                    ObjectType type = ParseObjectType(objectType, ObjectType.Activity, ObjectType.Agent, ObjectType.Group, ObjectType.Activity, ObjectType.StatementRef);
                    Object = type.CreateInstance(@object, version);
                }
                else if (@object["id"] != null)
                {
                    // Assume activity
                    Object = new Activity(@object, version);
                }
                else
                {
                    // TODO: Exception
                }
            }

            GuardAdditionalProperties((JObject)subStatement, "objectType", "actor", "object", "verb", "result", "context", "timestamp", "attachment");
        }

        public override bool Equals(object obj)
        {
            var statement = obj as SubStatement;
            return statement != null &&
                   base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = base.ToJToken(version, format);

            jobj["objectType"] = ObjectType.ToString();

            return jobj;
        }

        public static implicit operator SubStatement(JObject jobj)
        {
            return new SubStatement(jobj);
        }

        public static bool operator ==(SubStatement statement1, SubStatement statement2)
        {
            return EqualityComparer<SubStatement>.Default.Equals(statement1, statement2);
        }

        public static bool operator !=(SubStatement statement1, SubStatement statement2)
        {
            return !(statement1 == statement2);
        }
    }
}