using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using xApi.ApiUtils;
using xApi.Data;
using xApi.Data.InteractionTypes;

namespace xApi.Repositories
{
    public class ActivityRepository
    {
        private const string GetActivityDefinitionQuery = "SELECT * from dbo.ActivityDefinition "
           + "WHERE id = @id;";
        private const string GetActivityInteractionQuery = "SELECT * from dbo.ActivityInteraction "
           + "WHERE id = @id;";
        ActivityProfileRepository activityProfileRepository;
        public ActivityRepository()
        {
            activityProfileRepository = new ActivityProfileRepository();
        }
        public Activity GetActivity(Iri activityId)
        {
            int[] index = activityProfileRepository.GetActivityId(activityId);
            Activity activity = null;
            if (index[0] != -1)
            {
                activity = new Activity { Id = activityId };
            }
            if (index[1] != -1)
            {
                var definition = GetActivityDefinition(index[1]);
                activity.Definition = definition;
            }
            return activity;
        }

        private ActivityDefinition GetActivityDefinition(int index)
        {
            int interactionIndex = -1;
            StringBuilder sb = new StringBuilder("");
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {

                SqlDataReader reader = null;
                SqlCommand command = new SqlCommand(GetActivityDefinitionQuery, connection);
                command.Parameters.AddWithValue("@id", index);
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(6))
                        {
                            interactionIndex = reader.GetInt32(6);
                        }
                        if (!reader.IsDBNull(1))
                        {
                                sb.Append("\"name\":");
                            sb.Append(reader.GetString(1));
                            sb.Append(",");
                        }
                        if (!reader.IsDBNull(2))
                        {
                            sb.Append("\"description\":");
                            sb.Append(reader.GetString(2));
                            sb.Append(",");
                        }
                        if (!reader.IsDBNull(3))
                        {
                            sb.Append("\"type\":");
                            sb.Append("\"" + reader.GetString(3) + "\"");
                            sb.Append(",");
                        }
                        if (!reader.IsDBNull(4))
                        {
                            definition.moreInfo = new Uri(reader.GetString(4));
                        }
                        if (!reader.IsDBNull(5))
                        {
                            definition.extensions = new ExtensionsDictionary(reader.GetString(5));
                        }
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
            if (interactionIndex!=-1)
            {
                GetActivityInteraction(interactionIndex, definition);
            }
            return new ActivityDefinition(definition.toString());
        }

        private void GetActivityInteraction(int interactionIndex,dynamic definition)
        {
            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {

                SqlDataReader reader = null;
                SqlCommand command = new SqlCommand(GetActivityInteractionQuery, connection);
                command.Parameters.AddWithValue("@id", interactionIndex);
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        definition.interactionType = reader.GetString(1);
                        if (!reader.IsDBNull(2))
                        {
                        definition.correctResponsesPattern = new InteractionComponentCollection(reader.GetString(2));
                        }
                        if (!reader.IsDBNull(3))
                        {

                        }
                        if (!reader.IsDBNull(4))
                        {

                        }
                        if (!reader.IsDBNull(5))
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    return;
                }
                finally
                {
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }
        }
    }
}