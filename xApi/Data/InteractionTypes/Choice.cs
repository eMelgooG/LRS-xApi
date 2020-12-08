using Newtonsoft.Json.Linq;
using xApi.Data.Helpers;

namespace xApi.Data.InteractionTypes
{
    public class Choice : InteractionActivityDefinitionBase
    {
        protected override InteractionType INTERACTION_TYPE => InteractionType.Choice;
        public Choice() { }
        public Choice(JToken jobj, ApiVersion version) : base(jobj, version)
        {
            if (jobj["choices"] != null)
            {
                Choices = new InteractionComponentCollection(jobj["choices"], version);
            }
        }

        public InteractionComponentCollection Choices { get; set; }
    }
}
