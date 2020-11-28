using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Controllers
{
    public class AgentsController : XapiBaseController
    {
        AgentRepository _agentRepository;
        public AgentsController()
        {
            _agentRepository = new AgentRepository();
        }
    }
}