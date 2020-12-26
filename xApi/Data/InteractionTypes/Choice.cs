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
