using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using xApi.Data;
using xApi.Data.Documents;
using xApi.Data.Results;
using xApi.Repositories;

namespace xApi.Controllers
{
    [Route("xapi/activities/state")]
    public class StateProfileController : XapiBaseController
    {
        private StateActivityRepository stateActivityRepository;
        public StateProfileController()
        {
            stateActivityRepository = new StateActivityRepository();
        }

        [HttpGet]
        public IHttpActionResult GetState(
            [FromUri] Uri activityId = null,
           [FromUri] Agent agent = null,
           [FromUri] string stateId = null,
           Guid? registration = null,
            DateTimeOffset? since = null)
        {
            if (agent == null)
            {
                return BadRequest("Agent object needs to be provided.");
            }

            if (activityId == null)
            {
                return BadRequest("ActivityId parameter is missing.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StateProfileDocument profile = new StateProfileDocument { Activity = activityId, Agent = agent, StateId = stateId, Registration = registration};
            if (stateId != null)
            {
                 profile = stateActivityRepository.GetProfile(profile);
                if (profile == null)
                {
                    return NotFound();
                }
                return new DocumentResult(profile);
            }

            Object[] profiles = stateActivityRepository.GetProfiles(profile,since);
            if (profiles == null)
            {
                return Ok(new string[0]);
            }

            return new DocumentsResult(profiles);
        }
    }

}
