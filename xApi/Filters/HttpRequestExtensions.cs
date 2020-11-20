using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Controllers;

namespace xApi.Filters
{
    public static class HttpRequestExtensions
    {
        public static bool TryConcurrencyCheck(this HttpActionContext context, string savedEntityTag, DateTimeOffset? lastLodified, out HttpStatusCode statusCode)
        {
            statusCode = HttpStatusCode.PreconditionFailed;
            var headers = context.Request.Headers;
            var noneMatchList = headers.IfNoneMatch;
            var matchList = headers.IfMatch;
            if((noneMatchList == null || noneMatchList.Count < 1) && (matchList == null || matchList.Count < 1))
            {
                statusCode = HttpStatusCode.Conflict;
                return true;
            }
            if (noneMatchList.Count > 0)
            {
                foreach (var noneMatch in noneMatchList)
                {
                    if (string.IsNullOrEmpty(noneMatch.Tag))
                    {
                        continue;
                    }
                    if (noneMatch.Tag.Equals("*"))
                    {
                        if (!string.IsNullOrEmpty(savedEntityTag))
                        {
                            return true;
                        }
                    }
                    else if (noneMatch.Tag.Equals($"\"{savedEntityTag}\""))
                    {
                        return true;
                    }
                }
            }

            if (matchList.Count > 0)
            {
                foreach (var match in matchList)
                {
                    if (string.IsNullOrEmpty(match.Tag))
                    {
                        continue;
                    }

                    if (match.Tag.Equals("*"))
                    {
                        return true;
                    }
                    else if (!match.Tag.Equals($"\"{savedEntityTag}\""))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}