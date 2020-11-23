using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Documents;

namespace xApi.Repositories
{
    public class StateProfileRepository
    {
        public StateProfileRepository()
        {
        }

        public StateProfileDocument GetProfile(StateProfileDocument profile)
        {
            if (profile.Agent == null || profile.Activity == null) return null;
            return new StateProfileDocument();
        }

        public Object[] GetProfiles(StateProfileDocument agent, DateTimeOffset? since)
        {
            var result = new Object[2];
            result[0] = new List<String>() { "bla" };
            result[1] = DateTimeOffset.UtcNow;
            return result;
        }

        public void saveProfile(StateProfileDocument document)
        {

        }
        public void mergeProfiles(StateProfileDocument newDocument, StateProfileDocument oldDocument)
        {

        }

        public void DeleteProfile(StateProfileDocument profile)
        {
            throw new NotImplementedException();
        }

    }
}