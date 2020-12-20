using Newtonsoft.Json.Linq;
using xApi.Data.Helpers;

namespace xApi.Data.InteractionTypes
{
    public class Sequencing : InteractionActivityDefinitionBase
    {
        public Sequencing() { }

        public Sequencing(JToken jtoken, ApiVersion version) : base(jtoken, version)
        {
        }

        protected override InteractionType INTERACTION_TYPE => InteractionType.Sequencing;

        public InteractionComponentCollection Choices { get; set; }
    }
}
