using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace xApi.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http.Controllers;

    namespace xApi.Repositories
    {
        public class AgentProfileRepository
        {
            public AgentProfileRepository()
            {

            }


            public AgentProfileRepository GetProfile(Uri activityId, string profileId)
            {
                if (profileId == null) return null;
                return new AgentProfileRepository();
            }

            public Object[] GetProfiles(Uri activityId, DateTimeOffset? since)
            {
                var result = new Object[2];
                result[0] = new List<String>() { "bla" };
                result[1] = DateTimeOffset.UtcNow;
                return result;
            }

            public void saveProfile(AgentProfileRepository document)
            {

            }

            public void mergeProfiles(AgentProfileRepository newDocument, AgentProfileRepository oldDocument)
            {

            }

            public void DeleteProfile(AgentProfileRepository profile)
            {
                throw new NotImplementedException();
            }
        }
    }
}