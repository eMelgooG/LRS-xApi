using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Documents;

namespace xApi.Repositories
{
    public class ActivityProfileRepository
    {

        public ActivityProfileRepository()
        {
      
        }


        public ActivityProfileDocument GetProfile(Uri activityId, string profileId)
        {
            if (profileId == null) return null;
            return new ActivityProfileDocument();
        }

        public string[] GetProfiles(Uri activityId)
        {
            return new string[0];
        }
    }
}