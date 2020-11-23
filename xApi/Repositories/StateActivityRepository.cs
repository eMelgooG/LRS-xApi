using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Documents;

namespace xApi.Repositories
{
    public class StateActivityRepository
    {
        public StateActivityRepository()
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

    }
}