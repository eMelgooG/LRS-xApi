using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data;
using xApi.Data.Documents;

namespace xApi.Repositories
    {
        public class AgentProfileRepository
        {
            public AgentProfileRepository()
            {

            }


            public AgentProfileDocument GetProfile(Agent agent, string profileId)
            {
                if (profileId == null) return null;
                return new AgentProfileDocument();
            }

            public Object[] GetProfiles(Agent agent, DateTimeOffset? since)
            {
                var result = new Object[2];
                result[0] = new List<String>() { "bla" };
                result[1] = DateTimeOffset.UtcNow;
                return result;
            }

            public void saveProfile(AgentProfileDocument document)
            {

            }

            public void mergeProfiles(AgentProfileDocument newDocument, AgentProfileDocument oldDocument)
            {

            }

            public void DeleteProfile(AgentProfileDocument profile)
            {
                throw new NotImplementedException();
            }
        }
    }
