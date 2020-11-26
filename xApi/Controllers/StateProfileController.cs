using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using xApi.Data;
using xApi.Data.Documents;
using xApi.Data.Results;
using xApi.ApiUtils.Filters;
using xApi.ApiUtils.Binders.RawBody;
using xApi.Repositories;

namespace xApi.Controllers
{
    [Route("xapi/activities/state")]
    public class StateProfileController : XapiBaseController
    {
        private StateProfileRepository stateProfileRepository;
        public StateProfileController()
        {
            stateProfileRepository = new StateProfileRepository();
        }

        [HttpGet]
        public IHttpActionResult GetState(
            Iri activityId = null,
           Agent agent = null,
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

            StateProfileDocument profile = new StateProfileDocument { Activity = activityId, Agent = agent, StateId = stateId, Registration = registration };
            if (stateId != null)
            {
                profile = stateProfileRepository.GetProfile(profile);
                if (profile == null)
                {
                    return NotFound();
                }
                return new DocumentResult(profile);
            }

            Object[] profiles = stateProfileRepository.GetProfiles(profile, since);
            if (profiles == null)
            {
                return Ok(new string[0]);
            }

            return new DocumentsResult(profiles);
        }

        [HttpPost]
        [HttpPut]
        public IHttpActionResult SaveProfile(
           [RawBody] byte[] body,
            Iri activityId = null,
            Agent agent = null,
          [FromUri] string stateId = null,
           [FromUri] Guid? registration = null)
        {
            if (agent == null)
            {
                return BadRequest("Missing parameter agent");
            }
            if (stateId == null)
            {
                return BadRequest("Missing parameter stateId");
            }
            if (activityId == null)
            {
                return BadRequest("Missing parameter activityId");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string contenttype = null;
            if (this.Request.Content.Headers.Contains(RequiredContentTypeHeaderAttribute.CONTENT_TYPE))
            {
                contenttype = this.Request.Content.Headers.GetValues(RequiredContentTypeHeaderAttribute.CONTENT_TYPE).First();
            }

            //  connect to db and check if the profile already exists. Create it or if the profile exists try to merge.
            StateProfileDocument newDocument = new StateProfileDocument(body, contenttype)
            {
                Agent = agent,
                StateId = stateId,
                Registration = registration,
                Activity = activityId
            };

            var oldDocument = stateProfileRepository.GetProfile(newDocument);

            if (oldDocument != null)
            {
                /*  If method is Post -> merge documents:
                 *  1. check if both have the content type app/json
                 *  2. check for valid ETag
                 *  3. Try to merge
                 *  4. Return No Content
                 */
                if (this.Request.Method.Equals(HttpMethod.Post))
                {
                    if (!(oldDocument.ContentType.Equals(Constants.CONTENT_JSON) && oldDocument.ContentType.Equals(newDocument.ContentType)))
                    {
                        return BadRequest("Couldn't merge. The content-type needs to be application/json for both documents to be merged");
                    }

                    stateProfileRepository.mergeProfiles(newDocument, oldDocument);

                    return StatusCode(HttpStatusCode.NoContent);
                } 
            }
            //create or overwrite
            stateProfileRepository.saveProfile(newDocument);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        public IHttpActionResult DeleteProfile(
            Iri activityId = null,
            Agent agent = null,
          [FromUri] string stateId = null,
           [FromUri] Guid? registration = null,
           DateTimeOffset? since = null)
        {
            if (agent == null)
            {
                return BadRequest("Agent parameter needs to be provided.");
            }
            if (activityId == null)
            {
                return BadRequest("ProfileId parameter needs to be provided.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var profile = new StateProfileDocument()
            {
                Activity = activityId,
                Agent = agent,
                StateId = stateId,
                Registration = registration
            };

            //delete single document
            if (stateId != null)
            {
                profile = stateProfileRepository.GetProfile(profile);
                if (profile == null)
                {
                    return NotFound();
                }
                stateProfileRepository.DeleteProfile(profile);
                return StatusCode(HttpStatusCode.NoContent);
            }
            
            //delete multiple documents
            stateProfileRepository.DeleteProfiles(profile,since);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}

