using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace xApi.Data.Results
{
    public class ActivityProfilesResult : IHttpActionResult
    {
        Object[]_profiles = null;
        public ActivityProfilesResult(Object[] profiles)
        {

            _profiles = profiles;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ObjectContent<List<string>>((List<string>)_profiles[0], new JsonMediaTypeFormatter(), "application/json");
            response.Content.Headers.LastModified = (DateTimeOffset)_profiles[1];  
            return Task.FromResult(response);
        }
    }
}