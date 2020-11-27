using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xApi.Data.Helpers;

namespace xApi.Data.Interfaces
{
    public interface IStatementObject
    {
        ObjectType ObjectType { get; }

        JToken ToJToken(ApiVersion version, ResultFormat format);
    }
}
