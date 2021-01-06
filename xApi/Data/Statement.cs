using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using xApi.ApiUtils.Binders;
using xApi.Data.Helpers;
using xApi.Data.Interfaces;
using xApi.Data.Json;

namespace xApi.Data
{
    [ModelBinder(typeof(StatementModelBinder))]
    public class Statement : StatementBase, IStatement
    {
        public Statement() : base() { }
        public Statement(string jsonString) : this((JsonString)jsonString) { }
        public Statement(JsonString jsonString) : this(jsonString.ToJToken()) { }
        public Statement(JToken jtoken) : this(jtoken, ApiVersion.GetLatest()) { }
        public Statement(JToken statement, ApiVersion version) : base(statement, version)
        {
            GuardType(statement, JTokenType.Object);

            var id = statement["id"];
            if (id != null)
            {
                Id = ParseGuid(id);
            }

            var @object = statement["object"];
            if (@object != null)
            {
                GuardType(@object, JTokenType.Object);

                var jobjectType = @object["objectType"];
                if (jobjectType != null)
                {
                    ObjectType type = ParseObjectType(jobjectType, ObjectType.Activity, ObjectType.Agent, ObjectType.Group, ObjectType.Activity, ObjectType.StatementRef, ObjectType.SubStatement);
                    Object = type.CreateInstance(@object, version);
                }
                else if (@object["id"] != null)
                {
                    // Assume activity
                    Object = new Activity(@object, version);
                }
            }

            var stored = statement["stored"];
            if (stored != null)
            {
                Stored = ParseDateTimeOffset(stored);
            }

            var authority = statement["authority"];
            if (authority != null)
            {
                GuardType(authority, JTokenType.Object);

                var objectType = authority["objectType"];
                if (objectType != null)
                {
                    ObjectType type = ParseObjectType(objectType, ObjectType.Agent, ObjectType.Group);
                    Authority = (Agent)type.CreateInstance(authority, version);
                }
                else
                {
                    Authority = new Agent(authority, version);
                }

            }

            var ver = statement["version"];
            if (ver != null)
            {
                GuardType(ver, JTokenType.String);
                Version = new ApiVersion(ver.Value<string>());
            }

            GuardAdditionalProperties((JObject)statement, "id", "stored", "authority", "version", "object", "actor", "verb", "result", "context", "timestamp", "attachments");
        }



        /// <summary>
        /// UUID assigned by LRS if not set by the Learning Record Provider.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Timestamp of when this Statement was recorded. Set by LRS.
        /// </summary>
        public DateTimeOffset? Stored { get; set; }

        /// <summary>
        /// Agent or Group who is asserting this Statement is true.
        /// TODO: Verified by the LRS based on authentication.
        /// TODO: Set by LRS if not provided or if a strong trust relationship between the Learning Record Provider and LRS has not been established.
        /// </summary>
        public Agent Authority { get; set; }

        /// <summary>
        /// The Statement’s associated xAPI version, formatted according to Semantic Versioning 1.0.0.
        /// </summary>
        public ApiVersion Version { get; set; }

        public void Stamp()
        {
            Id = Id.HasValue ? Id : Guid.NewGuid();
            Timestamp = Timestamp.HasValue ? Timestamp : DateTime.UtcNow;
        }

        public override bool Equals(object obj)
        {
            var statement = obj as Statement;
            return statement != null &&
                   base.Equals(obj);
            //&&
            //EqualityComparer<Guid?>.Default.Equals(Id, statement.Id) &&
            //EqualityComparer<Agent>.Default.Equals(Authority, statement.Authority) &&
            //EqualityComparer<APIVersion>.Default.Equals(Version, statement.Version);
        }

        public override int GetHashCode()
        {
            var hashCode = -864523893;
            //hashCode = hashCode * -1521134295 + EqualityComparer<Guid?>.Default.GetHashCode(Id);
            //hashCode = hashCode * -1521134295 + EqualityComparer<Agent>.Default.GetHashCode(Authority);
            //hashCode = hashCode * -1521134295 + EqualityComparer<APIVersion>.Default.GetHashCode(Version);
            return hashCode;
        }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = base.ToJToken(version, format);
            if (Id != null)
            {
                obj["id"] = Id.ToString();
            }

            if (Stored != null)
            {
                obj["stored"] = Stored;
            }

            if (Authority != null)
            {
                obj["authority"] = Authority.ToJToken(version, format);
            }

            if (Version != null)
            {
                obj["version"] = Version.ToString();
            }

            return obj;
        }

        /// <summary>
        /// Convert the Statement to json using statement version
        /// </summary>
        /// <param name="format"></param>
        /// <returns>Statement as json</returns>
        public override string ToJson(ResultFormat format = ResultFormat.Exact)
        {
            // Override default version
            return base.ToJson(Version, format);
        }

        public override string ToJson()
        {
            // Override default version
            return base.ToJson(Version, ResultFormat.Exact);
        }

        public static bool operator ==(Statement statement1, Statement statement2)
        {
            return EqualityComparer<Statement>.Default.Equals(statement1, statement2);
        }

        public static bool operator !=(Statement statement1, Statement statement2)
        {
            return !(statement1 == statement2);
        }
    }
}