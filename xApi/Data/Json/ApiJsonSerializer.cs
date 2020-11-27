using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Helpers;

namespace xApi.Data.Json
{
    public class ApiJsonSerializer : Newtonsoft.Json.JsonSerializer
    {
        public ApiVersion Version { get; }
        public ResultFormat ResultFormat { get; }

        public ApiJsonSerializer()
            : this(ApiVersion.GetLatest())
        {
        }

        public ApiJsonSerializer(ApiVersion version)
            : this(version, ResultFormat.Exact)
        {
        }

        public ApiJsonSerializer(ApiVersion version, ResultFormat format)
        {
            Version = version;
            ResultFormat = format;
            CheckAdditionalContent = true;
            MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error;
            DateParseHandling = Newtonsoft.Json.DateParseHandling.None;
            DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
        }
    }
}
}