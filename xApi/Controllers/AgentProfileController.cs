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
    [Route("xapi/agents/profile")]
    public class AgentProfileController : XapiBaseController
    {
        private AgentProfileRepository agentProfileRepository;
        public AgentProfileController()
        {
            this.agentProfileRepository = new AgentProfileRepository();
        }
  

        [HttpGet]
        public IHttpActionResult GetProfiles(
            [FromUri] Agent agent = null,
             string profileId = null,
             DateTimeOffset? since = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (agent == null)
            {
                return BadRequest("Agent object needs to be provided.");
            }
            if (profileId != null)
            {
                AgentProfileDocument profile = agentProfileRepository.GetProfile(agent, profileId);
                if (profile == null)
                {
                    return NotFound();
                }
                return new DocumentResult(profile);
            }

            // otherwise we return the array of profileId's associated with that agent
            Object[] profiles = agentProfileRepository.GetProfiles(agent, since);
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
             [FromUri] Agent agent = null,
            [FromUri] string profileId = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (agent == null)
            {
                return BadRequest("Missing parameter agent");
            }
            if (profileId == null)
            {
                return BadRequest("Missing parameter profileId");
            }

            string contenttype = null;
            if (this.Request.Content.Headers.Contains(RequiredContentTypeHeaderAttribute.CONTENT_TYPE))
            {
                contenttype = this.Request.Content.Headers.GetValues(RequiredContentTypeHeaderAttribute.CONTENT_TYPE).First();
            }

            //  connect to db and check if the profile already exists. Create it or if the profile exists try to merge.


            AgentProfileDocument newDocument = new AgentProfileDocument(body, contenttype)
            {
                Agent = agent,
                ProfileId = profileId
            };

            var oldDocument = agentProfileRepository.GetProfile(agent, profileId);

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

                    if (this.ActionContext.TryConcurrencyCheck(oldDocument.Checksum, oldDocument.LastModified, out HttpStatusCode statusCode))
                    {
                        return StatusCode(statusCode);
                    }

                    oldDocument.MergeDocument(newDocument);
                    agentProfileRepository.OverwriteProfile(oldDocument);
                    return StatusCode(HttpStatusCode.NoContent);
                }

                /* If method is Put, replace the whole document
                 * 1. Check for ETag, return Conflict if it's not present or 415 if it doesn't match
                 */
                else if (this.Request.Method.Equals(HttpMethod.Put))
                {
                    if (this.ActionContext.TryConcurrencyCheck(oldDocument.Checksum, oldDocument.LastModified, out HttpStatusCode statusCode))
                    {
                        if (statusCode == HttpStatusCode.Conflict)
                        {
                            return Content(statusCode, $"-> check the current state of the resource.set the \"If - Match\" \n->header with the current ETag to resolve the conflict.");
                        }
                        return StatusCode(statusCode);
                    }
                    //overwrite
                    oldDocument.UpdateDocument(newDocument.Content, newDocument.ContentType);
                    agentProfileRepository.OverwriteProfile(oldDocument);
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }

            //create 
            agentProfileRepository.SaveProfile(newDocument);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        public IHttpActionResult DeleteProfile(
           [FromUri] Agent agent = null,
            string profileId = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (agent == null)
            {
                return BadRequest("Agent parameter needs to be provided.");
            }
            if (profileId == null)
            {
                return BadRequest("ProfileId parameter needs to be provided.");
            }
            AgentProfileDocument profile = agentProfileRepository.GetProfile(agent, profileId);
            if (profile == null)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                if (this.ActionContext.TryConcurrencyCheck(profile.Checksum, profile.LastModified, out var statusCode))
                {
                    return StatusCode(statusCode);
                }
            }
            agentProfileRepository.DeleteProfile(profile);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
