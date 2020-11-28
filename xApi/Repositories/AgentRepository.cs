using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data;

namespace xApi.Repositories
{
    public class AgentRepository
    {

       Person getPerson(Agent agent)
        {
            var person = new Person(agent);
            return person;
        }
    }
}