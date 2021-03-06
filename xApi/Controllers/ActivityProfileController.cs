﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.ModelBinding;
using xApi.Data;
using xApi.Data.Documents;
using xApi.Data.Helpers;
using xApi.Data.Results;
using xApi.ApiUtils.Filters;
using xApi.ApiUtils.Binders.RawBody;
using xApi.Repositories;

namespace xApi.Controllers
{
    /// <param name="activityId"></param>
    /// <param name="profileId"></param>
    /// <returns>200 OK, the Profile document</returns>
    [Route("xapi/activities/profile")]
    public class ActivityProfileController : XapiBaseController
    {
        private ActivityProfileRepository activityProfileRepository;
        public ActivityProfileController()
        {
            this.activityProfileRepository = new ActivityProfileRepository();
        }
        /// <summary>
        /// The semantics of the request are driven by the "profileId" parameter. 
        /// If it is included, the GET method will act upon a single defined document identified by "profileId". Otherwise, GET will return the available ids.
        /// </summary>
        /// <param name="activityId">The Activity id associated with this Profile document.</param>
        /// <param name="profileId">The profile id associated with this Profile document.</param>
        /// <param name="since">Only ids of Profile documents stored since the specified Timestamp (exclusive) are returned.</param>
        /// <returns></returns>

        [HttpGet]
        public IHttpActionResult GetProfiles(
             Iri activityId = null,
             string profileId = null,
             DateTimeOffset? since = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (activityId == null)
            {
                return BadRequest("ActivityId parameter needs to be provided.");
            }

            if (profileId != null) {
                ActivityProfileDocument profile = activityProfileRepository.GetProfile(activityId, profileId);
                if (profile == null)
                {
                    return NotFound();
                }
                return new DocumentResult(profile);
            }

            // otherwise we return the array of profileId's associated with that activity
            Object[] profiles = activityProfileRepository.GetProfiles(activityId, since);
            if (profiles == null)
            {
                return Ok(new string[0]);
            }
      
            return new DocumentsResult(profiles);

        }
    

        /// <summary>
        /// Stores or changes the specified Profile document in the context of the specified Activity.
        /// </summary>
        /// <param name="activityId">The Activity id associated with this Profile document.</param>
        /// <param name="profileId">The profile id associated with this Profile document.</param>
        /// <param name="document">The document to be stored or updated.</param>
        /// <returns>204 No Content</returns>
       [HttpPost]
       [HttpPut]
        public IHttpActionResult SaveProfile(
            [RawBody] byte[] body,
            [FromUri] Iri activityId=null,
           [FromUri] string profileId=null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (activityId==null)
            {
                return BadRequest("Missing parameter activityId");
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

            //  connect to db and check if the profile already exists.create it or if the profile already exists try to merge.


            ActivityProfileDocument newDocument = new ActivityProfileDocument(body,contenttype)
            {
                ActivityId = activityId,
                ProfileId = profileId
            };

            var oldDocument = activityProfileRepository.GetProfile(activityId, profileId);

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
                    activityProfileRepository.OverwriteProfile(oldDocument);
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
                    activityProfileRepository.OverwriteProfile(oldDocument);
                    return StatusCode(HttpStatusCode.NoContent);
                }

            }

            //create 
            activityProfileRepository.SaveProfile(newDocument);
       return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        public IHttpActionResult DeleteProfile(
            Iri activityId = null,
            string profileId = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (activityId == null)
            {
                return BadRequest("ActivityId parameter needs to be provided.");
            }
            if (profileId == null)
            {
                return BadRequest("ProfileId parameter needs to be provided.");
            }
                ActivityProfileDocument profile = activityProfileRepository.GetProfile(activityId, profileId);
                if (profile == null)
                {
                return StatusCode(HttpStatusCode.NoContent);
            } else
            {
                if(this.ActionContext.TryConcurrencyCheck(profile.Checksum,profile.LastModified, out var statusCode)) {
                    return StatusCode(statusCode);
                }
            }
            activityProfileRepository.DeleteProfile(profile);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}


