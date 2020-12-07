using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using xApi.ApiUtils;
using xApi.Data;
using xApi.Data.Documents;

namespace xApi.Repositories
    {

        public class AgentProfileRepository
        {
            public AgentProfileRepository()
            {

            }
        public const string GetAgentIdQuery = "SELECT TOP 1 id from dbo.Agent "
        + "WHERE {0} = @IFI ;";
        public const string GetAgentProfileIdQuery = "SELECT TOP 1 * from dbo.AgentProfile "
                + "WHERE profile_id = @profileId " +
                   "AND agent_id = @id ;";
        public const string GetProfilesIdsQuery = "SELECT profile_id, doc_last_modified from dbo.AgentProfile "
               + "WHERE agent_id = @id;";
        public const string CreateAgentQuery = "INSERT INTO dbo.Agent "
         + "(name,{0}) "
            + "OUTPUT inserted.id "
            + "VALUES (@name, @IFI);";
        public const string CreateAgentProfileQuery = "INSERT INTO dbo.AgentProfile "
 + "(profile_id, agent_id,doc_content_type,doc_content,doc_checksum,doc_last_modified,doc_created) "
    + "VALUES (@v1,@v2,@v3,@v4,@v5,@v6,@v7);";
        public const string UpdateAgentProfileQuery = "UPDATE dbo.AgentProfile "
        + "SET doc_content_type = @ctt, doc_content = @ct, doc_checksum = @cks, doc_last_modified = @lm "
            + "WHERE id = @id;";

        public AgentProfileDocument GetProfile(Agent agent, string profileId)
        {
            AgentProfileDocument document = null;
            int index = this.GetAgentId(agent);
            if (index != -1)
            {
                document = GetAgentProfileDocument(index, profileId);
            }
            return document;
        }
        public object[] GetProfiles(Agent agent, DateTimeOffset? since)
        {
            var result = new Object[2];
            var listResult = new List<AgentProfileDocument>();
            //check if the activity exists

            var index = this.GetAgentId(agent);
            if (index == -1) return null;
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                // get profiles             
                SqlCommand command = new SqlCommand(GetProfilesIdsQuery, connection);
                command.Parameters.AddWithValue("@id", index);
                SqlDataReader reader = null;
                try
                {
                    connection.Open();                
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        if (since != null)
                        {
                            while (reader.Read())
                            {
                                AgentProfileDocument curr = new AgentProfileDocument();
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
                                AgentProfileDocument curr = new AgentProfileDocument();
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
                    if (reader != null && !reader.IsClosed)
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

        public void OverwriteProfile(AgentProfileDocument document)
        {
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    // Create the activity               
                    SqlCommand command = new SqlCommand(UpdateAgentProfileQuery, connection);
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
        public void SaveProfile(AgentProfileDocument document)
        {
            int index = GetAgentId(document.Agent);
            //If agent doesn't exist we have to create it
            if (index == -1)
            {
                index = CreateAgent(document.Agent);
            }
            CreateProfile(index, document);

        }
        public void DeleteProfile(AgentProfileDocument profile)
        {
            throw new NotImplementedException();
        }
        public int CreateAgent(Agent agent)
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
                    SqlCommand command = new SqlCommand(String.Format(CreateAgentQuery,agent.GetIfiTypeName()), connection);
                    command.Parameters.AddWithValue("@IFI", agent.GetIfi());
                    command.Parameters.AddWithValue("@name", agent.Name);
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
        public AgentProfileDocument GetAgentProfileDocument(int agentId, string profileId)
        {
            AgentProfileDocument profileDoc = null;
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                SqlDataReader reader = null;
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(GetAgentProfileIdQuery, connection);
                    command.Parameters.AddWithValue("@profileId", profileId);
                    command.Parameters.AddWithValue("@id", agentId);
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        profileDoc = new AgentProfileDocument();

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
        public int GetAgentId(Agent agent)
        {
            int index = -1;
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                // get activity
                SqlCommand command = new SqlCommand(string.Format(GetAgentIdQuery, agent.GetIfiTypeName()), connection);
                command.Parameters.AddWithValue("@IFI", agent.GetIfi());
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
        public void CreateProfile(int agentId, AgentProfileDocument doc)
        {
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                // get activity
                SqlCommand command = new SqlCommand(CreateAgentProfileQuery, connection);
                command.Parameters.AddRange(new[]
    {
        new SqlParameter("@v1", doc.ProfileId),
        new SqlParameter("@v2", agentId),
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
