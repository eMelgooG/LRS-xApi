using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace xApi.Data.Helpers
{
    public static class Helpers
    {
        public static Dictionary<String, Object> parseJsonByteArrayToDictionary(byte[] json)
        {
            string jsonStr = Encoding.UTF8.GetString(json);
            return JsonConvert.DeserializeObject<Dictionary<String, Object>>(jsonStr);
        }
    }
}