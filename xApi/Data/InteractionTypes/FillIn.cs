using Newtonsoft.Json.Linq;
using xApi.Data.Helpers;

namespace xApi.Data.InteractionTypes
{
    public class FillIn : InteractionActivityDefinitionBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.FillIn;

        public FillIn()
        {
        }

        public FillIn(JToken jobj, ApiVersion version) : base(jobj, version)
        {
        }

    }
}
