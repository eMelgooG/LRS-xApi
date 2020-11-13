using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.ModelBinding;

namespace xApi.Controllers
{
    /// <param name="activityId"></param>
    /// <param name="profileId"></param>
    /// <returns>200 OK, the Profile document</returns>
    [Route("xapi/activities/profile")]
    public class ActivityProfileController : XapiBaseController
    {
        /// <summary>
        /// The semantics of the request are driven by the "profileId" parameter. 
        /// If it is included, the GET method will act upon a single defined document identified by "profileId". Otherwise, GET will return the available ids.
        /// </summary>
        /// <param name="activityId">The Activity id associated with this Profile document.</param>
        /// <param name="profileId">The profile id associated with this Profile document.</param>
        /// <param name="since">Only ids of Profile documents stored since the specified Timestamp (exclusive) are returned.</param>
        /// <returns></returns>

        [HttpGet]
        public IHttpActionResult GetActivityProfile(
             Uri activityId=null,
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

            //check to see if profileId was provided
            if (!string.IsNullOrEmpty(profileId))
            {
                //needs implementation
                return Ok("Nice");
            }
            else
            {
                //needs implementation
                return Ok(new string[0]);
            }

        }
    }
}
