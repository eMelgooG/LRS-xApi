using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Helpers;
using xApi.Data.Json;

namespace xApi.Data
{
    public class Context : JsonModel
    {
        public Context()
        {
        }
        public Context(JsonString jsonString) : this(jsonString.ToJToken(), ApiVersion.GetLatest())
        {
        }
        public Context(JToken context, ApiVersion version)
        {
            GuardType(context, JTokenType.Object);

            var registration = context["registration"];
            if (registration != null)
            {
                GuardType(registration, JTokenType.String);

                Registration = ParseGuid(registration);
            }

            var instructor = context["instructor"];
            if (instructor != null)
            {
                GuardType(instructor, JTokenType.Object);
                ObjectType objectType = ParseObjectType(instructor["objectType"], ObjectType.Agent, ObjectType.Group);

                if (objectType != null && objectType == ObjectType.Group)
                {
                    Instructor = new Group(instructor, version);
                }
                else
                {
                    Instructor = new Agent(instructor, version);
                }
            }

            var team = context["team"];
            if (team != null)
            {
                Instructor = new Group(team, version);
            }

            var contextActivities = context["contextActivities"];
            if (contextActivities != null)
            {
                ContextActivities = new ContextActivities(contextActivities, version);
            }

            var revision = context["revision"];
            if (revision != null)
            {
                GuardType(revision, JTokenType.String);
                Revision = revision.Value<string>();
            }

            var platform = context["platform"];
            if (platform != null)
            {
                GuardType(platform, JTokenType.String);
                Platform = platform.Value<string>();
            }

            var language = context["language"];
            if (language != null)
            {
                GuardType(language, JTokenType.String);
                Language = language.Value<string>();
            }

            var statement = context["statement"];
            if (statement != null)
            {
                ParseObjectType(statement["objectType"], ObjectType.StatementRef);
                Statement = new StatementRef(statement, version);
            }

            var extensions = context["extensions"];
            if (extensions != null)
            {
                Extensions = new ExtensionsDictionary(extensions, version);
            }
        }

        public Guid? Registration { get; set; }

        /// <summary>
        /// Instructor that the Statement relates to, if not included as the Actor of the Statement.
        /// </summary>
        public Agent Instructor { get; set; }

        /// <summary>
        /// Instructor that the Statement relates to, if not included as the Actor of the Statement.
        /// </summary>
        public Group Team { get; set; }

        /// <summary>
        /// A map of the types of learning activity context that this Statement is related to. Valid context types are: parent, "grouping", "category" and "other".
        /// </summary>
        public ContextActivities ContextActivities { get; set; }

        /// <summary>
        /// Revision of the learning activity associated with this Statement. Format is free.
        /// The "revision" property MUST only be used if the Statement's Object is an Activity.
        /// </summary>
        public string Revision { get; set; }

        /// <summary>
        /// Platform used in the experience of this learning activity.
        /// The "platform" property MUST only be used if the Statement's Object is an Activity.
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Code representing the language in which the experience being recorded in this Statement (mainly) occurred in, if applicable and known.
        /// </summary>
        public string Language { get; set; }


        /// <summary>
        /// Another Statement to be considered as context for this Statement.
        /// </summary>
        public StatementRef Statement { get; set; }

        public ExtensionsDictionary Extensions { get; set; }

        public override bool Equals(object obj)
        {
            var context = obj as Context;
            return context != null &&
                   EqualityComparer<Guid?>.Default.Equals(Registration, context.Registration) &&
                   EqualityComparer<Agent>.Default.Equals(Instructor, context.Instructor) &&
                   EqualityComparer<Group>.Default.Equals(Team, context.Team) &&
                   EqualityComparer<ContextActivities>.Default.Equals(ContextActivities, context.ContextActivities) &&
                   Revision == context.Revision &&
                   Platform == context.Platform &&
                   Language == context.Language &&
                   EqualityComparer<StatementRef>.Default.Equals(Statement, context.Statement) &&
                   EqualityComparer<ExtensionsDictionary>.Default.Equals(Extensions, context.Extensions);
        }

        public override int GetHashCode()
        {
            var hashCode = 2045218665;
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid?>.Default.GetHashCode(Registration);
            hashCode = hashCode * -1521134295 + EqualityComparer<Agent>.Default.GetHashCode(Instructor);
            hashCode = hashCode * -1521134295 + EqualityComparer<Group>.Default.GetHashCode(Team);
            hashCode = hashCode * -1521134295 + EqualityComparer<ContextActivities>.Default.GetHashCode(ContextActivities);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Revision);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Platform);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Language);
            hashCode = hashCode * -1521134295 + EqualityComparer<StatementRef>.Default.GetHashCode(Statement);
            hashCode = hashCode * -1521134295 + EqualityComparer<ExtensionsDictionary>.Default.GetHashCode(Extensions);
            return hashCode;
        }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = new JObject();
            if (Registration.HasValue)
            {
                jobj["registration"] = Registration;
            }

            if (Instructor != null)
            {
                jobj["instructor"] = Instructor.ToJToken(version, format);
            }

            if (Instructor != null)
            {
                jobj["instructor"] = Instructor.ToJToken(version, format);
            }

            if (Team != null)
            {
                jobj["team"] = Team.ToJToken(version, format);
            }

            if (ContextActivities != null)
            {
                jobj["contextActivities"] = ContextActivities.ToJToken(version, format);
            }

            if (Revision != null)
            {
                jobj["revision"] = Revision;
            }

            if (Platform != null)
            {
                jobj["platform"] = Platform;
            }

            if (Language != null)
            {
                jobj["language"] = Language;
            }

            if (Statement != null)
            {
                jobj["statement"] = Statement.ToJToken(version, format);
            }

            if (Extensions != null)
            {
                jobj["extensions"] = Extensions.ToJToken(version, format);
            }

            return jobj;
        }

        public static bool operator ==(Context context1, Context context2)
        {
            return EqualityComparer<Context>.Default.Equals(context1, context2);
        }

        public static bool operator !=(Context context1, Context context2)
        {
            return !(context1 == context2);
        }
    }
}