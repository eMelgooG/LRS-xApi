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
        public IHttpActionResult GetProfiles(
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
            if (profileId!=null)
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

       

         /// <summary>
        /// Stores or changes the specified Profile document in the context of the specified Activity.
        /// </summary>
        /// <param name="activityId">The Activity id associated with this Profile document.</param>
        /// <param name="profileId">The profile id associated with this Profile document.</param>
        /// <param name="document">The document to be stored or updated.</param>
        /// <returns>204 No Content</returns>
        //[HttpPost]
        //[HttpPut]
        //public IHttpActionResult SaveProfile(
        //    string profileId,
        //    Uri activityId,
        //    [BindRequired, FromHeader(Name = "Content-Type")] string contentType,
        //    [BindRequired, FromBody] byte[] body,
        //    [FromUri] Guid? registration = null)
        //{
        //    if (profileId == null)
        //    {
        //        return BadRequest("ProfileId parameter needs to be provided.");
        //    }
        //    if(activityId==null)
        //    {
        //        return BadRequest("ActivityId parameter needs to be provided.");
        //    }
        //    if()

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var profile = await _mediator.Send(new GetActivityProfileQuery()
        //    {
        //        ActivityId = activityId,
        //        ProfileId = profileId,
        //        Registration = registration
        //    }, cancellationToken);

        //    if (Request.TryConcurrencyCheck(profile?.Document.Checksum, profile?.Document.LastModified, out int statusCode))
        //    {
        //        return StatusCode(statusCode);
        //    }

        //    if (profile != null)
        //    {
        //        // Optimistic Concurrency
        //        if (HttpMethods.IsPut(Request.Method))
        //        {
        //            return Conflict();
        //        }

        //        await _mediator.Send(new UpdateActivityProfileCommand()
        //        {

        //            ProfileId = profileId,
        //            ActivityId = activityId,
        //            Content = body,
        //            ContentType = contentType,
        //            Registration = registration
        //        }, cancellationToken);
        //    }
        //    else
        //    {
        //        await _mediator.Send(new CreateActivityProfileCommand()
        //        {
        //            ProfileId = profileId,
        //            ActivityId = activityId,
        //            Content = body,
        //            ContentType = contentType,
        //            Registration = registration
        //        }, cancellationToken);
        //    }

        //    return NoContent();
        //}
    }
}
