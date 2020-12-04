using System;
using System.Collections.Generic;
using System.Data;
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
        public const string GetProfileIdQuery = "SELECT TOP 1 * from dbo.ActivityProfile "
                + "WHERE profile_id = @profileId " +
                   "AND activity_id = @id ;";
        public const string GetProfilesIdsQuery = "SELECT profile_id, doc_last_modified from dbo.ActivityProfile "
               + "WHERE activity_id = @id;";
        public const string CreateActivityQuery = "INSERT INTO dbo.Activity "
         + "(activity_iri) "
            + "OUTPUT inserted.id "
            + "VALUES (@activity_id);";
        public const string CreateActivityProfileQuery = "INSERT INTO dbo.ActivityProfile "
 + "(profile_id, activity_id,doc_content_type,doc_content,doc_checksum,doc_last_modified,doc_created) "
    + "VALUES (@v1,@v2,@v3,@v4,@v5,@v6,@v7);";
        public const string UpdateActivityProfileQuery = "UPDATE dbo.ActivityProfile "
        + "SET doc_content_type = @ctt, doc_content = @ct, doc_checksum = @cks, doc_last_modified = @lm "
            + "WHERE id = @id;"; 

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
            ActivityProfileDocument document = null;
            int index = this.GetActivityId(activityId);
            if (index != -1)
            {
                document = GetActivityProfileDocument(index, profileId);
            }
            return document;
        }
        public object[] GetProfiles(Iri activityId, DateTimeOffset? since)
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
                                if (curr.LastModified > since)
                                {
                                    listResult.Add(curr);
                                }
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                ActivityProfileDocument curr = new ActivityProfileDocument();
                                curr.ProfileId = reader.GetString(0);
                                curr.LastModified = reader.GetDateTimeOffset(1);
                                listResult.Add(curr);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Profiles associated with activity, not found");
                    }

                }
                catch (Exception ex)
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

        public void OverwriteProfile(ActivityProfileDocument document)
        {
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    // Create the activity               
                    SqlCommand command = new SqlCommand(UpdateActivityProfileQuery, connection);
                    command.Parameters.AddWithValue("@ctt", document.ContentType);
                    command.Parameters.AddWithValue("@ct", document.Content);
                    command.Parameters.AddWithValue("@cks", document.Checksum);
                    command.Parameters.AddWithValue("@lm", document.LastModified);
                    command.Parameters.AddWithValue("@id", document.Id);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                finally
                {
  
                }
            }
        }
        public void SaveProfile(ActivityProfileDocument document)
        {
            int index = GetActivityId(document.ActivityId);
            //If activity doesn't exist we have to create it
            if (index == -1)
            {
                index = CreateActivity(document.ActivityId);
            }
            CreateProfile(index, document);

        }
        public void mergeProfiles(ActivityProfileDocument newDocument,ActivityProfileDocument oldDocument)
        {

        }
        public void DeleteProfile(ActivityProfileDocument profile)
        {
            throw new NotImplementedException();
        }
        public int CreateActivity(Iri activity)
        {
            int index = -1;
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                // get activity
                SqlDataReader reader = null;
                try
                {
                    connection.Open();
                    // Create the activity               
                    SqlCommand command = new SqlCommand(CreateActivityQuery, connection);
                    command.Parameters.AddWithValue("@activity_id", activity._iriString);
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        index = reader.GetInt32(0);
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }
            return index;
        }
        public ActivityProfileDocument GetActivityProfileDocument(int activityId, string profileId)
        {
            ActivityProfileDocument profileDoc = null;
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                SqlDataReader reader = null;
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(GetProfileIdQuery, connection);
                    command.Parameters.AddWithValue("@profileId", profileId);
                    command.Parameters.AddWithValue("@id", activityId);
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        profileDoc = new ActivityProfileDocument();

                        if (!reader.IsDBNull(0))
                        {
                            profileDoc.Id = (int)reader[0];
                        }
                        if (!reader.IsDBNull(1))
                        {
                            profileDoc.ProfileId = (string)reader[1];
                        }
 /*                       if (!reader.IsDBNull(2))
                        {
                            profileDoc.ActivityId = (int)reader[2];
                        }*/
                        if (!reader.IsDBNull(3))
                        {
                            profileDoc.ContentType = (string)reader[3];
                        }
                        profileDoc.Content = DbUtils.GetBytes(reader, 4);
                        if (!reader.IsDBNull(5))
                        {
                            profileDoc.Checksum = reader.GetString(5);
                        }
                        if (!reader.IsDBNull(6))
                        {
                            profileDoc.LastModified = (DateTimeOffset)reader[6];
                        }
                        return profileDoc;
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }
            return profileDoc;
        }
        public int GetActivityId(Iri activity)
        {
            int index = -1;
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                // get activity
                SqlCommand command = new SqlCommand(GetActivityIdQuery, connection);
                command.Parameters.AddWithValue("@activityId", activity._iriString);
                SqlDataReader reader = null;
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        index = reader.GetInt32(0);
                    }
                }
                catch (Exception ex)
                {


                }
                finally
                {
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }
            return index;

        }
        public void CreateProfile(int activityId, ActivityProfileDocument doc)
        {
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                // get activity
                SqlCommand command = new SqlCommand(CreateActivityProfileQuery, connection);
                command.Parameters.AddRange(new[]
    {
        new SqlParameter("@v1", doc.ProfileId),
        new SqlParameter("@v2", activityId),
         new SqlParameter("@v3", doc.ContentType),
          new SqlParameter("@v4", doc.Content),
           new SqlParameter("@v5", doc.Checksum),
            new SqlParameter("@v6", doc.LastModified),
             new SqlParameter("@v7", doc.CreateDate),
    });
                try
                {
                    connection.Open();
                   command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
            }
        }

    }
}