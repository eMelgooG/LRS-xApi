using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xApi.Data.Interfaces
{
    public interface IStatementsResult
    {
        Uri More { get; set; }
        StatementCollection Statements { get; set; }
    }
}
