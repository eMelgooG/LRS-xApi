using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xApi.Data.Interfaces
{
    public interface IJsonData
    {
        string ToJson();

        string ToJson(ResultFormat format = ResultFormat.Exact);
    }
}
