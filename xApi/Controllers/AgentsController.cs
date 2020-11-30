using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using xApi.Data;
using xApi.Repositories;

namespace xApi.Controllers
{
    [Route("xapi/agents")]
    public class AgentsController : XapiBaseController
    {
        AgentRepository _agentRepository;
        public AgentsController()
        {
            _agentRepository = new AgentRepository();
        }

        
        [HttpGet]
        public IHttpActionResult Get(Agent agent = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (agent == null)
            {
                return BadRequest("Please provide an agent object as a parameter.");
            }
            var person = _agentRepository.GetPerson(agent);
            return Ok(person);
        }
    }
}