using Newtonsoft.Json.Linq;
using xApi.Data.Helpers;

namespace xApi.Data.InteractionTypes
{
    public class TrueFalse : InteractionActivityDefinitionBase
    {
        public TrueFalse()
        {
        }

        public TrueFalse(JToken jobj, ApiVersion version) : base(jobj, version)
        {
        }

        protected override InteractionType INTERACTION_TYPE => InteractionType.TrueFalse;
    }
}
