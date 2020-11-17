using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using xApi.Data.Documents;

namespace xApi.Data.Results
{
    public class ActivityProfileResult : IHttpActionResult
    {
        ActivityProfileDocument _profile = null;
        public ActivityProfileResult(ActivityProfileDocument profile)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            _profile = profile;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(_profile.Content);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(_profile.ContentType);
            return Task.FromResult(response);
        }
    }
}