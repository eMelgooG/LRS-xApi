using Newtonsoft.Json.Linq;
using xApi.Data.Helpers;

namespace xApi.Data.InteractionTypes
{
    public class Performance : InteractionActivityDefinitionBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.Performance;

        public Performance() { }

        public Performance(JToken jobj, ApiVersion version) : base(jobj, version)
        {
            if (jobj["steps"] != null)
            {
                Steps = new InteractionComponentCollection(jobj.Value<JArray>("steps"), version);
            }
        }
        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = base.ToJToken(version, format);
            if (this.Steps != null)
            {
                obj["steps"] = Steps.ToJToken(version, format);
            }
            return obj;
        }

        public InteractionComponentCollection Steps { get; set; }
    }
}
