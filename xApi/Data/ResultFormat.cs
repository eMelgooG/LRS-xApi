using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace xApi.Data
{
    public enum ResultFormat
    {
        [EnumMember(Value = "ids")]
        Ids = 0,

        [EnumMember(Value = "exact")]
        Exact = 1,

        [EnumMember(Value = "canonical")]
        Canonical = 2
    }
}