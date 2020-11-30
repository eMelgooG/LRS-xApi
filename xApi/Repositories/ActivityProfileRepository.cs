using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using xApi.ApiUtils;
using xApi.Data;
using xApi.Data.Documents;

namespace xApi.Repositories
{
    public class ActivityProfileRepository
    {
        public const string GetActivityIdQuery = "SELECT TOP 1 id from dbo.Activity "
                + "WHERE activity_iri = @activityId ;";
        public const string GetProfileIdQuery = "SELECT TOP 1 doc_content, doc_content_type, doc_last_modified from dbo.ActivityProfile "
                + "WHERE profile_id = @profileId " +
                   "AND activity_id = @id ;";

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
            //check if the activity exists
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                // get activity
                SqlCommand command = new SqlCommand(GetActivityIdQuery, connection);
                command.Parameters.AddWithValue("@activityId", activityId._iriString);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        reader.Close();
                        return null;
                    }

                    var index = reader[0];
                    reader.Close();
                    //get profile
                    command = new SqlCommand(GetProfileIdQuery, connection);
                    command.Parameters.AddWithValue("@profileId", profileId);
                    command.Parameters.AddWithValue("@id", index);
                    reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        reader.Close();
                        return null;
                    }
                    var profileDoc = new ActivityProfileDocument();
                    if (!reader.IsDBNull(0)) {
                        profileDoc.Content = (byte[])reader[0];
                    }
                    if (!reader.IsDBNull(1))
                    {
                        profileDoc.ContentType = (string)reader[1];
                    }
                    if (!reader.IsDBNull(2))
                    {
                        profileDoc.LastModified = (DateTimeOffset)reader[2];
                    }

                    reader.Close();
                    return profileDoc;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
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