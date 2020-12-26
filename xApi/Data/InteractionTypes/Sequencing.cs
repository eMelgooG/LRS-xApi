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

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = base.ToJToken(version, format);
            if (this.Choices != null)
            {
                obj["choices"] = Choices.ToJToken(version, format);
            }
            return obj;
        }
        public InteractionComponentCollection Choices { get; set; }
    }
}
