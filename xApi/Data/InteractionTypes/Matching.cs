using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xApi.Data.Helpers;

namespace xApi.Data.InteractionTypes
{
    public class Matching : InteractionActivityDefinitionBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.Matching;

        public Matching()
        {
        }
        public Matching(JToken jobj, ApiVersion version) : base(jobj, version)
        {
            if (jobj["source"] != null)
            {
                Source = new InteractionComponentCollection(jobj.Value<JArray>("source"), version);
            }

            if (jobj["target"] != null)
            {
                Target = new InteractionComponentCollection(jobj.Value<JArray>("target"), version);
            }
        }
        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = base.ToJToken(version, format);
            if (this.Source != null)
            {
                obj["source"] = Source.ToJToken(version, format);
            }
            if (this.Target != null)
            {
                obj["target"] = Target.ToJToken(version, format);
            }
            return obj;
        }


        /*   [JsonProperty("source")]*/
        public InteractionComponentCollection Source { get; set; }

    /*    [JsonProperty("target")]*/
        public InteractionComponentCollection Target { get; set; }
    }
}
