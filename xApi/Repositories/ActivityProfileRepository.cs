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
        public const string GetProfileIdQuery = "SELECT TOP 1 doc_content, doc_content_type, doc_last_modified, doc_checksum from dbo.ActivityProfile "
                + "WHERE profile_id = @profileId " +
                   "AND activity_id = @id ;";
        public const string GetProfilesIdsQuery = "SELECT profile_id, doc_last_modified from dbo.ActivityProfile "
               + "WHERE activity_id = @id;";

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

                    profileDoc.Content = DbUtils.GetBytes(reader, 0);
                    if (!reader.IsDBNull(1))
                    {
                        profileDoc.ContentType = (string)reader[1];
                    }
                    if (!reader.IsDBNull(2))
                    {
                        profileDoc.LastModified = (DateTimeOffset)reader[2];
                    }
                    if (!reader.IsDBNull(3))
                    {
                        profileDoc.Tag = reader.GetString(3);
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
             var listResult = new List<ActivityProfileDocument>();
                        //check if the activity exists
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                // get activity
                SqlCommand command = new SqlCommand(GetActivityIdQuery, connection);
                command.Parameters.AddWithValue("@activityId", activityId._iriString);
                SqlDataReader reader = null;
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    if (!reader.Read())
                    {
                        throw new Exception("Activity IRI not found");
                    }

                    var index = reader[0];
                    reader.Close();
                    //get profile
                    command = new SqlCommand(GetProfilesIdsQuery, connection);
                    command.Parameters.AddWithValue("@id", index);
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        if (since != null)
                        {
                            while (reader.Read())
                            {
                                ActivityProfileDocument curr = new ActivityProfileDocument();
                                curr.ProfileId = reader.GetString(0);
                                curr.LastModified = reader.GetDateTimeOffset(1);
                                if(curr.LastModified > since)
                                {
                                    listResult.Add(curr);
                                }
                            }
                        } else
                        {
                            while (reader.Read())
                            {
                                ActivityProfileDocument curr = new ActivityProfileDocument();
                                curr.ProfileId = reader.GetString(0);
                                curr.LastModified = reader.GetDateTimeOffset(1);
                                listResult.Add(curr);
                            }
                        }
                    } else
                    {
                        throw new Exception("Profiles associated with activity, not found");
                    }
                
                } catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
            listResult.Sort((x, y) => Nullable.Compare(y.LastModified, x.LastModified));
            result[1] = listResult.First().LastModified;
            result[0] = listResult.Select(i => i.ProfileId).ToList();
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