using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using xApi.Data;
using xApi.Data.Documents;

namespace xApi.Repositories
{
    public class ActivityProfileRepository
    {

        /*     string jsonStr = "{\r\n    \"name\": \"13d2dfc9-04cf-44f9-832-f53c04c6dcfe\",\r\n    \"location\": {\r\n                \"name\": \"5b6f1721-b9-4bc1-8ec5-87363da5be38\"\r\n    },\r\n    \"2e0618a7-c1d9-43f5-b9ed-201c6f1d08a5\": \"aed267b8-93ba-42e3-bffc-a20a3151d7a0\"\r\n}";
Dictionary<String,Object> dic = JsonConvert.DeserializeObject<Dictionary<String, Object>>(jsonStr);
Dictionary<String, Object> dic2 = Helpers.parseJsonByteArrayToDictionary(body);
foreach (KeyValuePair<string, object> entry in dic2)
{
bool x = dic.ContainsKey(entry.Key);
bool y = dic[entry.Key].ToString().Equals(dic2[entry.Key].ToString()) ? true : false;
int z = 1;
}*/
        public ActivityProfileRepository()
        {
      
        }


        public ActivityProfileDocument GetProfile(Iri activityId, string profileId)
        {
            if (profileId == null) return null;
            return new ActivityProfileDocument();
        }

        public Object[] GetProfiles(Iri activityId, DateTimeOffset? since)
        {
            var result = new Object[2];
            result[0] = new List<String>() { "bla" };
            result[1] = DateTimeOffset.UtcNow;
            return result;
        }

        public void saveProfile(ActivityProfileDocument document)
        {
            
        }

        public void mergeProfiles(ActivityProfileDocument newDocument, ActivityProfileDocument oldDocument)
        {

        }

        public void DeleteProfile(ActivityProfileDocument profile)
        {
            throw new NotImplementedException();
        }
    }
}