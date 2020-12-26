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
    public class Verb : JsonModel, IVerb
    {
        public Verb() { }
        public Verb(JsonString jsonString) : this(jsonString.ToJToken())
        {
        }
        public Verb(JToken verb) : this(verb, ApiVersion.GetLatest())
        {
        }
        public Verb(JToken verb, ApiVersion version)
        {
            GuardType(verb, JTokenType.Object);

            var id = verb["id"];
            if (id != null)
            {
                GuardType(id, JTokenType.String);
                Id = new Iri(id.Value<string>());
            }

            var display = verb["display"];
            if (display != null)
            {
                Display = new LanguageMap(display, version);
            }
        }

        /// <summary>
        /// Corresponds to a Verb definition. (Required)
        /// Each Verb definition corresponds to the meaning of a Verb, not the word.
        /// </summary>
        public Iri Id { get; set; }

        public LanguageMap Display { get; set; }

        public override bool Equals(object obj)
        {
            var verb = obj as Verb;
            return verb != null &&
                   EqualityComparer<Iri>.Default.Equals(Id, verb.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<Iri>.Default.GetHashCode(Id);
        }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = new JObject
            {
                ["id"] = Id.ToString(),
            };

            if (Display != null && format != ResultFormat.Ids)
            {
                jobj["display"] = Display.ToJToken(version, format);
            }

            return jobj;
        }

        public static bool operator ==(Verb verb1, Verb verb2)
        {
            return EqualityComparer<Verb>.Default.Equals(verb1, verb2);
        }

        public static bool operator !=(Verb verb1, Verb verb2)
        {
            return !(verb1 == verb2);
        }
    }
}