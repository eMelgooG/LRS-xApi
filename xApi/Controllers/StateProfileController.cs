﻿using System;
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
    public class StateProfileController : ApiController
    {
        private StateProfileRepository _stateProfileRepository;
        public StateProfileController()
        {
            _stateProfileRepository = new StateProfileRepository();
        }

        [HttpGet]
        public IHttpActionResult GetState(
            Iri activityId = null,
           Agent agent = null,
           [FromUri] string stateId = null,
           Guid? registration = null,
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

            if (activityId == null)
            {
                return BadRequest("ActivityId parameter is missing.");
            }

            StateProfileDocument mock = new StateProfileDocument { Activity = activityId, Agent = agent, StateId = stateId, Registration = registration };
            StateProfileDocument profile = null;
            if (stateId != null)
            {
                profile = _stateProfileRepository.GetProfile(mock);
                if (profile == null)
                {
                    return NotFound();
                }
                return new DocumentResult(profile);
            }

            Object[] profiles = _stateProfileRepository.GetProfiles(mock, since);
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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

            var oldDocument = _stateProfileRepository.GetProfile(newDocument);

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

                    oldDocument.MergeDocument(newDocument);
                    _stateProfileRepository.OverwriteProfile(oldDocument);
                    return StatusCode(HttpStatusCode.NoContent);
                }
                else if (this.Request.Method.Equals(HttpMethod.Put))
                {
                    oldDocument.UpdateDocument(newDocument.Content, newDocument.ContentType);
                    _stateProfileRepository.OverwriteProfile(oldDocument);
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            //create
            _stateProfileRepository.SaveProfile(newDocument);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        public IHttpActionResult DeleteProfile(
            Iri activityId = null,
            Agent agent = null,
          [FromUri] string stateId = null,
           [FromUri] Guid? registration = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (agent == null)
            {
                return BadRequest("Agent parameter needs to be provided.");
            }
            if (activityId == null)
            {
                return BadRequest("ProfileId parameter needs to be provided.");
            }
            var mock = new StateProfileDocument()
            {
                Activity = activityId,
                Agent = agent,
                StateId = stateId,
                Registration = registration
            };
            StateProfileDocument profile = null;
            //delete single document
            if (stateId != null)
            {
               profile = _stateProfileRepository.GetProfile(mock);
                _stateProfileRepository.DeleteProfile(profile);
                return StatusCode(HttpStatusCode.NoContent);
            }
            
            //delete multiple documents
            _stateProfileRepository.DeleteProfiles(mock);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}

