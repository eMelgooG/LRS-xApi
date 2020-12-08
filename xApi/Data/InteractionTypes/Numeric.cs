using Newtonsoft.Json.Linq;
using xApi.Data.Helpers;

namespace xApi.Data.InteractionTypes
{
    public class Numeric : InteractionActivityDefinitionBase
    {
        public Numeric() { }

        public Numeric(JToken jobj, ApiVersion version) : base(jobj, version)
        {
        }

        protected override InteractionType INTERACTION_TYPE => InteractionType.Numeric;
    }
}
