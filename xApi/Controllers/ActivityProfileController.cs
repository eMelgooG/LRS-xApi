using Newtonsoft.Json;
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
using xApi.Data.Documents;
using xApi.Data.Helpers;
using xApi.Data.Results;
using xApi.Filters;
using xApi.Filters.RawBody;
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
             Uri activityId = null,
             string profileId = null,
             DateTimeOffset? since = null)
        {
            if (activityId == null)
            {
                return BadRequest("ActivityId parameter needs to be provided.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (profileId != null) {
                ActivityProfileDocument profile = activityProfileRepository.GetProfile(activityId, profileId);
                if (profile == null)
                {
                    return NotFound();
                }
                return new ActivityProfileResult(profile);
            }

            // otherwise we return the array of profileId's associated with that activity
            string[] profiles = activityProfileRepository.GetProfiles(activityId);
            return Ok(profiles);

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
           [FromUri] string profileId=null,
           [FromUri] Uri activityId=null,
            [FromUri] Guid? registration = null)
        {
       
            if (profileId == null)
            {
                return BadRequest("profileid parameter needs to be provided.");
            }
            if (activityId == null)
            {
                return BadRequest("activityid parameter needs to be provided.");
            }
            if (body == null)
            {
                return BadRequest("content needs to be provided in body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contenttype = this.Request.Content.Headers.GetValues(RequiredContentTypeHeaderAttribute.CONTENT_TYPE).First();
            //  connect to db and check if the profile already exists.create it or if the profile already exists try to merge.
            ActivityProfileDocument document = new ActivityProfileDocument()
            {
                ActivityId = activityId,
                ProfileId = profileId,
                Registration = registration,
                Content = body,
                ContentType = contenttype
            };

            return Ok();
            }
    }
}


