using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Controllers;

namespace xApi.ApiUtils.Filters
{
    public static class HttpRequestExtensions
    {
        public static bool TryConcurrencyCheck(this HttpActionContext context, string savedEntityTag, DateTimeOffset? lastLodified, out HttpStatusCode statusCode)
        {
            statusCode = HttpStatusCode.PreconditionFailed;
            var headers = context.Request.Headers;
            var noneMatchList = headers.IfNoneMatch;
            var matchList = headers.IfMatch;
            var method = context.Request.Method;
            
            if (method.Equals(HttpMethod.Put))
            {
                if (Check409(matchList, noneMatchList))
                {
                    statusCode = HttpStatusCode.Conflict;
                    return true;
                }
                if(CheckIfMatchValid(matchList,savedEntityTag) || CheckIfNonMatchValid(noneMatchList, savedEntityTag))
                {
                    return true;
                }
            }
            else if (method.Equals(HttpMethod.Post))
            {
                if (CheckIfMatchValid(matchList, savedEntityTag) || CheckIfNonMatchValid(noneMatchList, savedEntityTag))
                {
                    return true;
                }
            }
            else if (method.Equals(HttpMethod.Delete))
            {
                if (CheckIfMatchValid(matchList, savedEntityTag))
                {
                    return true;
                }
            }



            return false;
        }

        private static bool Check409(HttpHeaderValueCollection<EntityTagHeaderValue> matchList, HttpHeaderValueCollection<EntityTagHeaderValue> noMatchList)
        {
            return ((noMatchList == null || noMatchList.Count < 1) && (matchList == null || matchList.Count < 1));
        }

        private static bool CheckIfMatchValid(HttpHeaderValueCollection<EntityTagHeaderValue> matchList, string savedEntityTag)
        {
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

        private static bool CheckIfNonMatchValid(HttpHeaderValueCollection<EntityTagHeaderValue> noneMatchList, string savedEntityTag)
        {
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
            return false;
        }


    }
}