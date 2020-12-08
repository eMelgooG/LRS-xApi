using Newtonsoft.Json.Linq;
using xApi.Data.Helpers;
using xApi.Data.Json;

namespace xApi.Data.InteractionTypes
{
    public class InteractionComponent : JsonModel
    {
        public InteractionComponent() { }

        public InteractionComponent(JsonString jsonString) : this(jsonString.ToJToken(), ApiVersion.GetLatest()) { }

        public InteractionComponent(JToken jobj, ApiVersion version)
        {
            var id = jobj["id"];
            if (id != null)
            {
                GuardType(id, JTokenType.String);
                Id = id.Value<string>();
            }

            var description = jobj["description"];
            if (description != null)
            {
                Description = new LanguageMap(description, version);
            }
        }

        public string Id { get; set; }

        public LanguageMap Description { get; set; }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = new JObject();
            if (!string.IsNullOrEmpty(Id))
            {
                jobj["id"] = Id;
            }

            if (Description != null)
            {
                jobj["description"] = Description.ToJToken(version, format);
            }
            return jobj;
        }
    }
}
