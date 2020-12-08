using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using xApi.Data.Helpers;
using xApi.Data.Json;

namespace xApi.Data.InteractionTypes
{
    public class InteractionComponentCollection : JsonModel<JArray>, ICollection<InteractionComponent>
    {
        public ICollection<InteractionComponent> Components = new HashSet<InteractionComponent>();

        public InteractionComponentCollection()
        {
        }

        public InteractionComponentCollection(JsonString jsonString) : this(jsonString.ToJToken(), ApiVersion.GetLatest())
        {
        }

        public InteractionComponentCollection(JToken jarr, ApiVersion version)
        {
            foreach (var item in jarr)
            {
                Add(new InteractionComponent(item, version));
            }
        }

        public int Count => Components.Count;

        public bool IsReadOnly => Components.IsReadOnly;

        public void Add(InteractionComponent item)
        {
            Components.Add(item);
        }

        public void Clear()
        {
            Components.Clear();
        }

        public bool Contains(InteractionComponent item)
        {
            return Components.Contains(item);
        }

        public void CopyTo(InteractionComponent[] array, int arrayIndex)
        {
            Components.CopyTo(array, arrayIndex);
        }

        public IEnumerator<InteractionComponent> GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        public bool Remove(InteractionComponent item)
        {
            return Components.Remove(item);
        }

        public override JArray ToJToken(ApiVersion version, ResultFormat format)
        {
            var jarr = new JArray();
            foreach (var component in Components)
            {
                jarr.Add(component.ToJToken(version, format));
            }
            return jarr;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Components.GetEnumerator();
        }
    }
}
