using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using xApi.Data.Exceptions;
using xApi.Data.Helpers;

namespace xApi.Data.InteractionTypes
{
    public class InteractionType
    {
        private static readonly ICollection<InteractionType> _types = new HashSet<InteractionType>();

        public static readonly InteractionType Choice = new InteractionType("choice", typeof(Choice));
        public static readonly InteractionType FillIn = new InteractionType("fill-in", typeof(FillIn));
        public static readonly InteractionType Likert = new InteractionType("likert", typeof(Likert));
        public static readonly InteractionType LongFillIn = new InteractionType("long-fill-in", typeof(LongFillIn));
        public static readonly InteractionType Matching = new InteractionType("matching", typeof(Matching));
        public static readonly InteractionType Numeric = new InteractionType("numeric", typeof(Numeric));
        public static readonly InteractionType Performance = new InteractionType("performance", typeof(Performance));
        public static readonly InteractionType Sequencing = new InteractionType("sequencing", typeof(Sequencing));
        public static readonly InteractionType TrueFalse = new InteractionType("true-false", typeof(TrueFalse));
        public static readonly InteractionType Other = new InteractionType("other", typeof(Other));

        public readonly string Alias;
        public readonly Type Type;

        private InteractionType(string strType, Type type)
        {
            Alias = strType;
            Type = type;
            _types.Add(this);
        }


        public ActivityDefinition CreateInstance(JToken jtoken, ApiVersion version)
        {
            if (Type == typeof(Choice))
            {
                return new Choice(jtoken, version);
            }

            else if (Type == typeof(FillIn))
            {
                return new FillIn(jtoken, version);
            }
            else if (Type == typeof(Likert))
            {
                return new Likert(jtoken, version);
            }
            else if (Type == typeof(LongFillIn))
            {
                return new LongFillIn(jtoken, version);
            }
            else if (Type == typeof(Matching))
            {
                return new Matching(jtoken, version);
            }
            else if (Type == typeof(Numeric))
            {
                return new Numeric(jtoken, version);
            }
            else if (Type == typeof(Performance))
            {
                return new Performance(jtoken, version);
            }
            else if (Type == typeof(Sequencing))
            {
                return new Sequencing(jtoken, version);
            }
            else if (Type == typeof(TrueFalse))
            {
                return new TrueFalse(jtoken, version);
            }
            else if (Type == typeof(Other))
            {
                return new Other(jtoken, version);
            }

            throw new NotImplementedException("interactionType");
        }
        public ActivityDefinition CreateInstanceNoValidation()
        {
            if (Type == typeof(Choice))
            {
                return new Choice();
            }

            else if (Type == typeof(FillIn))
            {
                return new FillIn();
            }
            else if (Type == typeof(Likert))
            {
                return new Likert();
            }
            else if (Type == typeof(LongFillIn))
            {
                return new LongFillIn();
            }
            else if (Type == typeof(Matching))
            {
                return new Matching();
            }
            else if (Type == typeof(Numeric))
            {
                return new Numeric();
            }
            else if (Type == typeof(Performance))
            {
                return new Performance();
            }
            else if (Type == typeof(Sequencing))
            {
                return new Sequencing();
            }
            else if (Type == typeof(TrueFalse))
            {
                return new TrueFalse();
            }
            else if (Type == typeof(Other))
            {
                return new Other();
            }

            throw new NotImplementedException("interactionType");
        }
        public override bool Equals(object obj)
        {
            return obj is InteractionType type &&
                   Alias == type.Alias;
        }

        public override int GetHashCode()
        {
            return -331038658 + EqualityComparer<string>.Default.GetHashCode(Alias);
        }

        public static bool operator ==(InteractionType type1, InteractionType type2)
        {
            return type1.Alias == type2.Alias;
        }

        public static bool operator !=(InteractionType type1, InteractionType type2)
        {
            return type1?.Alias != type2?.Alias;
        }

        public static implicit operator InteractionType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            var interactionType = _types.FirstOrDefault(x => x.Alias == type);
            if (interactionType != null)
            {
                return interactionType;
            }

            throw new InteractionTypeException(type);
        }

        public static implicit operator string(InteractionType type)
        {
            return type.Alias;
        }
    }
}
