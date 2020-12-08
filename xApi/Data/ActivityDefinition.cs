using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Helpers;
using xApi.Data.Json;
using xApi.Data.Json.Exceptions;

namespace xApi.Data
{
    public class ActivityDefinition : JsonModel
    {
        public ActivityDefinition() { }
        public ActivityDefinition(JsonString jsonString) : this(jsonString.ToJToken(), ApiVersion.GetLatest()) { }
        public ActivityDefinition(JToken obj, ApiVersion version)
        {
            GuardType(obj, JTokenType.Object);

            var type = obj["type"];
            if (type != null)
            {
                GuardType(type, JTokenType.String);
                Type = new Iri(type.Value<string>());
            }

            var moreInfo = obj["moreInfo"];
            if (moreInfo != null)
            {
                GuardType(moreInfo, JTokenType.String);
                try
                {
                    MoreInfo = new Uri(moreInfo.Value<string>());
                }
                catch (UriFormatException ex)
                {
                    throw new JsonTokenModelException(moreInfo, ex);
                }
            }

            var name = obj["name"];
            if (name != null)
            {
                Name = new LanguageMap(name, version);
            }

            var description = obj["description"];
            if (description != null)
            {
                Description = new LanguageMap(description, version);
            }

            var extensions = obj["extensions"];
            if (extensions != null)
            {
                Extensions = new ExtensionsDictionary(extensions, version);
            }
        }

        public LanguageMap Name { get; set; }

        public LanguageMap Description { get; set; }

        public virtual Iri Type { get; set; }

        /// <summary>
        /// Resolves to a document with human-readable information about the Activity, which could include a way to launch the activity.
        /// </summary>
        public Uri MoreInfo { get; set; }

        public ExtensionsDictionary Extensions { get; set; }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = new JObject();

            if (Type != null)
            {
                obj["type"] = Type.ToString();
            }

            if (Name != null)
            {
                obj["name"] = Name.ToJToken(version, format);
            }

            if (Description != null)
            {
                obj["description"] = Description.ToJToken(version, format);
            }

            if (MoreInfo != null)
            {
                obj["moreInfo"] = MoreInfo.ToString();
            }

            if (Extensions != null)
            {
                obj["extensions"] = Extensions.ToJToken(version, format);
            }

            return obj;
        }

        public override bool Equals(object obj)
        {
            var definition = obj as ActivityDefinition;
            return definition != null &&
                   base.Equals(obj) &&
                   //EqualityComparer<LanguageMap>.Default.Equals(Name, definition.Name) &&
                   //EqualityComparer<LanguageMap>.Default.Equals(Description, definition.Description) &&
                   EqualityComparer<Iri>.Default.Equals(Type, definition.Type) &&
                   EqualityComparer<Uri>.Default.Equals(MoreInfo, definition.MoreInfo);
        }

        public override int GetHashCode()
        {
            var hashCode = -1346148704;
            //hashCode = hashCode * -1521134295 + EqualityComparer<LanguageMap>.Default.GetHashCode(Name);
            //hashCode = hashCode * -1521134295 + EqualityComparer<LanguageMap>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + EqualityComparer<Iri>.Default.GetHashCode(Type);
            hashCode = hashCode * -1521134295 + EqualityComparer<Uri>.Default.GetHashCode(MoreInfo);
            return hashCode;
        }

        public static bool operator ==(ActivityDefinition definition1, ActivityDefinition definition2)
        {
            return EqualityComparer<ActivityDefinition>.Default.Equals(definition1, definition2);
        }

        public static bool operator !=(ActivityDefinition definition1, ActivityDefinition definition2)
        {
            return !(definition1 == definition2);
        }
    }
}