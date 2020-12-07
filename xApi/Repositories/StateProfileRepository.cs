using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using xApi.ApiUtils;
using xApi.Data.Documents;

namespace xApi.Repositories
{
    public class StateProfileRepository
    {
        public const string GetStateProfileQuery = "SELECT TOP 1 * from dbo.StateProfile "
         + "WHERE state_id = @stateId " +
            "AND agent_id = @agentId " +
             "AND activity_id = @activityId " +
            "{0};";
        public const string GetStateProfilesQuery = "SELECT state_id, doc_last_modified from dbo.StateProfile "
   +   "WHERE agent_id = @agentId " +
       "AND activity_id = @activityId " +
      "{0};";
        public const string UpdateStateProfileQuery = "UPDATE dbo.StateProfile "
+ "SET doc_content_type = @ctt, doc_content = @ct, doc_checksum = @cks, doc_last_modified = @lm "
    + "WHERE id = @id;";
        public const string CreateStateProfileQuery = "INSERT INTO dbo.StateProfile "
+ "(state_id, activity_id, agent_id, registration_id, doc_content_type,doc_content,doc_checksum,doc_last_modified,doc_created) "
+ "VALUES (@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9);";
        public const string DeleteMultipleProfilesQuery = "DELETE FROM dbo.StateProfile " +
            "WHERE activity_id = @actId " +
            "AND agent_id = @agentId " +
            "{0};";
        public const string DeleteSingleProfileQuery = "DELETE FROM dbo.StateProfile " +
            "WHERE id = @id;";
        public const string RegistrationQuery = "AND registration_id = @regId ";
        private readonly AgentProfileRepository _agentProfileRepository;
        private readonly ActivityProfileRepository _activityProfileRepository;
        public StateProfileRepository()
        {
            _agentProfileRepository = new AgentProfileRepository();
            _activityProfileRepository = new ActivityProfileRepository();
        }

        public StateProfileDocument GetProfile(StateProfileDocument profile)
        {
            StateProfileDocument result = null;
            int agentId = _agentProfileRepository.GetAgentId(profile.Agent);
            int activityId = _activityProfileRepository.GetActivityId(profile.Activity);
            if(agentId!=-1 && activityId !=-1)
            {
                result = GetStateProfileDocument(agentId, activityId, profile.StateId, profile.Registration);
            }
            return result;
        }

        private StateProfileDocument GetStateProfileDocument(int agentId, int activityId, string stateId, Guid? registration)
        {
            StateProfileDocument profileDoc = null;
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                SqlDataReader reader = null;
                try
                {
                    connection.Open();
                    SqlCommand command = null;
                    //dynamic query if registration is not null
                    if(registration!=null)
                    {
                      command = new SqlCommand(String.Format(GetStateProfileQuery,RegistrationQuery), connection);
                        command.Parameters.AddWithValue("@regId", registration);
                    } else
                    {
                        command = new SqlCommand(String.Format(GetStateProfileQuery, ""), connection);
                    }

                    command.Parameters.AddWithValue("@stateId", stateId);
                    command.Parameters.AddWithValue("@agentId", agentId);
                    command.Parameters.AddWithValue("@activityId", activityId);
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        profileDoc = new StateProfileDocument();

                        if (!reader.IsDBNull(0))
                        {
                            profileDoc.Id = (int)reader[0];
                        }

                        profileDoc.StateId = stateId;
                        profileDoc.Registration = registration;
                       
                        if (!reader.IsDBNull(5))
                        {
                            profileDoc.ContentType = (string)reader[3];
                        }
                        profileDoc.Content = DbUtils.GetBytes(reader, 6);
                        if (!reader.IsDBNull(7))
                        {
                            profileDoc.Checksum = reader.GetString(7);
                        }
                        if (!reader.IsDBNull(8))
                        {
                            profileDoc.LastModified = reader.GetDateTimeOffset(8);
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
        public object[] GetProfiles(StateProfileDocument state, DateTimeOffset? since)
        {
            var result = new Object[2];
            var listResult = new List<StateProfileDocument>();
            var actId = _activityProfileRepository.GetActivityId(state.Activity);
            if (actId == -1) return null;
            var agentId = _agentProfileRepository.GetAgentId(state.Agent);
            if (agentId == -1) return null;
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                // get profiles             
                SqlCommand command = null;
                if (state.Registration != null)
                {
                    command = new SqlCommand(String.Format(GetStateProfilesQuery, RegistrationQuery ), connection);
                    command.Parameters.AddWithValue("@regId", state.Registration);
                }
                else
                {
                    command = new SqlCommand(String.Format(GetStateProfilesQuery, ""), connection);
                }
                command.Parameters.AddWithValue("@agentId", agentId);
                command.Parameters.AddWithValue("@activityId", actId);

                SqlDataReader reader = null;
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    if (since == null)
                    {
                        since = DateTimeOffset.MinValue;
                    }
                    if (reader.HasRows)
                    {
                        while (reader.Read())

                        {
                            StateProfileDocument curr = new StateProfileDocument();
                            curr.StateId = reader.GetString(0);
                            curr.LastModified = reader.GetDateTimeOffset(1);
                            if (curr.LastModified > since)
                            {
                                listResult.Add(curr);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("State profiles not found");
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
            result[0] = listResult.Select(i => i.StateId).ToList();
            return result;
        }
       public void OverwriteProfile(StateProfileDocument document)
        {
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    // Create the activity               
                    SqlCommand command = new SqlCommand(UpdateStateProfileQuery, connection);
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
        public void SaveProfile(StateProfileDocument document)
        {
            int agentIndex = _agentProfileRepository.GetAgentId(document.Agent);
            //If agent doesn't exist we have to create it
            if (agentIndex == -1)
            {
                agentIndex = _agentProfileRepository.CreateAgent(document.Agent);
            }
            int activityIndex = _activityProfileRepository.GetActivityId(document.Activity);
            if (agentIndex == -1)
            {
                activityIndex = _activityProfileRepository.CreateActivity(document.Activity);
            }
            CreateProfile(agentIndex, activityIndex,document);
        }
        public void CreateProfile(int agent, int activity, StateProfileDocument doc)
        {

            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                // get activity
                SqlCommand command = new SqlCommand(CreateStateProfileQuery, connection);
                command.Parameters.AddRange(new[]
    {
        new SqlParameter("@v1", doc.StateId),
          new SqlParameter("@v2", activity),
        new SqlParameter("@v3", agent),
         new SqlParameter("@v4", doc.Registration ?? (object)DBNull.Value),
         new SqlParameter("@v5", doc.ContentType),
          new SqlParameter("@v6", doc.Content),
           new SqlParameter("@v7", doc.Checksum),
            new SqlParameter("@v8", doc.LastModified),
             new SqlParameter("@v9", doc.CreateDate),
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
        public void DeleteProfile(StateProfileDocument profile)
        {
            if (profile == null) return;
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                SqlCommand command = null;
                command = new SqlCommand(DeleteSingleProfileQuery, connection);
                command.Parameters.AddWithValue("@id", profile.Id);
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
        public void DeleteProfiles(StateProfileDocument profile)
        {
            var agentIndex = _agentProfileRepository.GetAgentId(profile.Agent);
            if (agentIndex == -1) return;
            var activityIndex = _activityProfileRepository.GetActivityId(profile.Activity);
            if (activityIndex == -1) return;
            using(SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                SqlCommand command = null;
                if(profile.Registration!=null)
                {
                    command = new SqlCommand(String.Format(DeleteMultipleProfilesQuery, RegistrationQuery), connection);
                    command.Parameters.AddWithValue("@regId", profile.Registration);
                } else
                {
                    command = new SqlCommand(String.Format(DeleteMultipleProfilesQuery, ""), connection);
                }
                command.Parameters.AddWithValue("@actId", activityIndex);
                command.Parameters.AddWithValue("@agentId", agentIndex);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                } catch (Exception ex)
                {

                }
            }
        }
      
    }
}