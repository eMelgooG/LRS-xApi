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
   
             response.Content = _profile.Content!=null? new ByteArrayContent(_profile.Content) : new ByteArrayContent(new byte[0]);  
            response.Content.Headers.ContentType = _profile.ContentType!=null ? new MediaTypeHeaderValue(_profile.ContentType) : null;
            response.Content.Headers.LastModified = _profile.LastModified;
            EntityTagHeaderValue etag = new EntityTagHeaderValue("\"" + _profile.Tag + "\"");
            response.Headers.ETag = etag;
            return Task.FromResult(response);
        }
    }
}