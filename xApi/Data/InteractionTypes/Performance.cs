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

        public InteractionComponentCollection Steps { get; set; }
    }
}
