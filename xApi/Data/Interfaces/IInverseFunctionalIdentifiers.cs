using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xApi.Data.Interfaces
{
    interface IInverseFunctionalIdentifiers
    {
        Account Account { get; set; }
        Mbox Mbox { get; set; }
        string Mbox_SHA1SUM { get; set; }
        string Name { get; set; }
        Iri OpenId { get; set; }
    }
}
