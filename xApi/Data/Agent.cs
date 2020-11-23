using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data
{
    public class Agent
    {
        public string ObjectType { get; set; }
        public string Name { get; set; }
        public Mbox Mbox { get; set; }
        public string Mbox_SHA1SUM { get; set; }
        public Uri OpenId { get; set; }

        public Account Account { get; set; }
    }

}