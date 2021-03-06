﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Data.Interfaces
{
    public interface IAgent
    {
        ObjectType ObjectType { get; }
        Account Account { get; set; }
        Mbox Mbox { get; set; }
        string Mbox_SHA1SUM { get; set; }
        string Name { get; set; }
        Iri OpenId { get; set; }
    }
}