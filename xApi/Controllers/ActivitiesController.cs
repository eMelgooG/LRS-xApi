using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using xApi.Data;
using xApi.Repositories;

namespace xApi.Controllers
{
    [Route("xapi/activities")]
    public class ActivitiesController : ApiController
    {
        private ActivityRepository activityRepository;
        public ActivitiesController()
        {
            this.activityRepository = new ActivityRepository();
        }
        [HttpGet]
        public IHttpActionResult GetActivity(
              [FromUri] Iri activityId = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (activityId == null)
            {
                return BadRequest("ActivityId parameter required.");
            }
            var activity = activityRepository.GetActivity(activityId);
            if (activity == null)
            {
                return Ok(new Activity());
            }

            ResultFormat format = ResultFormat.Canonical;
/*            ResultFormat format = ResultFormat.Exact;
            if (this.Request.Headers.AcceptLanguage != null && this.Request.Headers.AcceptLanguage.Count > 0)
            {
                format = ResultFormat.Canonical;
            }*/

            return Ok(activity.ToJson(format));

        }
    }
}
