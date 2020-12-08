using Newtonsoft.Json.Linq;
using xApi.Data.Helpers;

namespace xApi.Data.InteractionTypes
{
    public class Likert : InteractionActivityDefinitionBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.Likert;

        public Likert()
        {
        }
        public Likert(JObject jobj, ApiVersion version) : base(jobj, version)
        {
            if (jobj["scale"] != null)
            {
                Scale = new InteractionComponentCollection(jobj.Value<JArray>("scale"), version);
            }
        }

        public Likert(JToken jobj, ApiVersion version) : base(jobj, version)
        {
        }

        public InteractionComponentCollection Scale { get; set; }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = base.ToJToken(version, format);

            return jobj;
        }
    }
}
