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
    public abstract class StatementBase : JsonModel<JToken>, IStatementBase, IAttachmentByHash
    {
        public StatementBase() : base(null, null)
        {
            Attachments = new AttachmentCollection();
        }

        public StatementBase(JToken jobj) : this(jobj, ApiVersion.GetLatest()) { }

        public StatementBase(JToken statement, ApiVersion version) : base(statement, version)
        {
            Attachments = new AttachmentCollection();

            GuardType(statement, JTokenType.Object);

            var actor = statement["actor"];
            if (actor != null)
            {
                GuardType(actor, JTokenType.Object);

                var objectType = actor["objectType"];
                if (objectType != null)
                {
                    ObjectType type = ParseObjectType(objectType, ObjectType.Agent, ObjectType.Group);
                    Actor = (Agent)type.CreateInstance(actor, version);
                }
                else
                {
                    Actor = new Agent(actor, version);
                }
            }

            var verb = statement["verb"];
            if (verb != null)
            {
                Verb = new Verb(verb, version);
            }

            if (statement["result"] != null)
            {
                Result = new Result(statement["result"], version);
            }

            if (statement["context"] != null)
            {
                Context = new Context(statement["context"], version);
            }

            var timestamp = statement["timestamp"];
            if (timestamp != null)
            {
                Timestamp = ParseDateTimeOffset(timestamp);
            }

            if (statement["attachments"] != null)
            {
                Attachments = new AttachmentCollection(statement["attachments"], version);
            }
        }

        /// <summary>
        /// Whom the Statement is about, as an Agent or Group Object.
        /// </summary>
        public Agent Actor { get; set; }

        /// <summary>
        /// Action taken by the Actor.
        /// </summary>
        public Verb Verb { get; set; }

        /// <summary>
        /// Activity, Agent, or another Statement that is the Object of the Statement.
        /// </summary>
        public IStatementObject Object { get; set; }

        /// <summary>
        /// Result Object, further details representing a measured outcome.
        /// </summary>
        public Result Result { get; set; }

        /// <summary>
        /// Context that gives the Statement more meaning. Examples: a team the Actor is working with, altitude at which a scenario was attempted in a flight simulator.
        /// </summary>
        public Context Context { get; set; }

        /// <summary>
        /// Timestamp of when the events described within this Statement occurred. Set by the LRS if not provided.
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }

        public AttachmentCollection Attachments { get; set; }

        public override bool Equals(object obj)
        {
            var @base = obj as StatementBase;
            return @base != null &&
                   base.Equals(obj) &&
                   EqualityComparer<Agent>.Default.Equals(Actor, @base.Actor) &&
                   EqualityComparer<Verb>.Default.Equals(Verb, @base.Verb) &&
                   EqualityComparer<IStatementObject>.Default.Equals(Object, @base.Object) &&
                   EqualityComparer<Result>.Default.Equals(Result, @base.Result) &&
                   EqualityComparer<Context>.Default.Equals(Context, @base.Context);
        }

        public override int GetHashCode()
        {
            var hashCode = -1199450156;
            hashCode = hashCode * -1521134295 + EqualityComparer<Agent>.Default.GetHashCode(Actor);
            hashCode = hashCode * -1521134295 + EqualityComparer<Verb>.Default.GetHashCode(Verb);
            hashCode = hashCode * -1521134295 + EqualityComparer<IStatementObject>.Default.GetHashCode(Object);
            hashCode = hashCode * -1521134295 + EqualityComparer<Result>.Default.GetHashCode(Result);
            hashCode = hashCode * -1521134295 + EqualityComparer<Context>.Default.GetHashCode(Context);
            return hashCode;
        }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = new JObject();

            if (Actor != null)
            {
                jobj["actor"] = Actor.ToJToken(version, format);
            }

            if (Verb != null)
            {
                jobj["verb"] = Verb.ToJToken(version, format);
            }

            if (Object != null)
            {
                jobj["object"] = Object.ToJToken(version, format);
            }

            if (Result != null)
            {
                jobj["result"] = Result.ToJToken(version, format);
            }

            if (Context != null)
            {
                jobj["context"] = Context.ToJToken(version, format);
            }

            if (Timestamp.HasValue)
            {
                jobj["timestamp"] = Timestamp?.ToString("o");
            }

            if (Attachments != null && Attachments.Count > 0)
            {
                jobj["attachments"] = Attachments.ToJToken(version, format);
            }

            return jobj;
        }

        public Attachment GetAttachmentByHash(string sha2)
        {
            if (Attachments == null || Attachments.Count <= 0)
            {
                return null;
            }

            foreach (var attachment in Attachments)
            {
                if (attachment.SHA2 == sha2)
                {
                    return attachment;
                }
            }

            return null;
        }

        public static bool operator ==(StatementBase base1, StatementBase base2)
        {
            return EqualityComparer<StatementBase>.Default.Equals(base1, base2);
        }

        public static bool operator !=(StatementBase base1, StatementBase base2)
        {
            return !(base1 == base2);
        }
    }
}