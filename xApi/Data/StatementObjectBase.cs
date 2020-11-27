using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Helpers;
using xApi.Data.Json;

namespace xApi.Data
{
    public abstract class StatementObjectBase : JsonModel
    {
        protected StatementObjectBase() { }
        protected StatementObjectBase(JToken jobj, ApiVersion version)
        {
        }
        protected abstract ObjectType OBJECT_TYPE { get; }
        public ObjectType ObjectType { get { return this.OBJECT_TYPE; } }
        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            return new JObject
            {
                ["objectType"] = (string)ObjectType
            };
        }
        public override bool Equals(object obj)
        {
            var @base = obj as StatementObjectBase;
            return @base != null &&
                   base.Equals(obj) &&
                   OBJECT_TYPE == @base.OBJECT_TYPE;
        }

        public static bool operator ==(StatementObjectBase base1, StatementObjectBase base2)
        {
            return EqualityComparer<StatementObjectBase>.Default.Equals(base1, base2);
        }

        public static bool operator !=(StatementObjectBase base1, StatementObjectBase base2)
        {
            return !(base1 == base2);
        }

        public override int GetHashCode()
        {
            var hashCode = 1521006493;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + OBJECT_TYPE.GetHashCode();
            return hashCode;
        }
    }
}