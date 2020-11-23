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
    public class DocumentResult : IHttpActionResult
    {
        IDocument _profile = null;
        public DocumentResult(IDocument profile)
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
            response.Content.Headers.LastModified = _profile.LastModified;
            response.Content.Headers.Add("ETag", $"'{_profile.Tag}'");
            return Task.FromResult(response);
        }
    }
}