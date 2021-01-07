using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using xApi.Data.Exceptions;
using xApi.Data.Helpers;
using xApi.Data.Interfaces;

namespace xApi.Data
{
    public class ObjectType
    {
        private static readonly ICollection<ObjectType> _types = new HashSet<ObjectType>();

        public static readonly ObjectType Agent = new ObjectType("Agent", typeof(Agent));
        public static readonly ObjectType Group = new ObjectType("Group", typeof(Group));
        public static readonly ObjectType Activity = new ObjectType("Activity", typeof(Activity));
        public static readonly ObjectType SubStatement = new ObjectType("SubStatement", typeof(SubStatement));
        public static readonly ObjectType StatementRef = new ObjectType("StatementRef", typeof(StatementRef));

        public readonly string Alias;
        public readonly Type Type;

        private ObjectType(string alias, Type type)
        {
            Alias = alias;
            Type = type;
            _types.Add(this);
        }

        public IStatementObject CreateInstance(JToken jsonToken, ApiVersion version)
        {
            if (Type == typeof(Agent))
            {
                return new Agent(jsonToken, version);
            }

            else if (Type == typeof(Group))
            {
                return new Group(jsonToken, version);
            }

            else if (Type == typeof(Activity))
            {
                return new Activity(jsonToken, version);
            }

            else if (Type == typeof(SubStatement))
            {
                return new SubStatement(jsonToken, version);
            }

            else if (Type == typeof(StatementRef))
            {
                return new StatementRef(jsonToken, version);
            }

            throw new NotImplementedException("objectType");
        }

        public override string ToString()
        {
            return Alias;
        }

        public override bool Equals(object obj)
        {
            return obj is ObjectType type &&
                   Alias == type.Alias;
        }

        public override int GetHashCode()
        {
            var hashCode = 1278524668;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Alias);
            return hashCode;
        }

        public static implicit operator string(ObjectType type)
        {
            return type.ToString();
        }

        public static implicit operator ObjectType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            var objectType = _types.FirstOrDefault(x => x.Alias == type);
            if (objectType != null)
            {
                return objectType;
            }

            throw new ObjectTypeException(type);
        }

        public static bool operator ==(ObjectType left, ObjectType right)
        {
            return left?.ToString() == right?.ToString();
        }

        public static bool operator !=(ObjectType left, ObjectType right)
        {
            return left?.ToString() != right?.ToString();
        }
    }
}
