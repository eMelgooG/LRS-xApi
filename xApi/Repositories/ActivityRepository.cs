using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using xApi.ApiUtils;
using xApi.Data;
using xApi.Data.Helpers;
using xApi.Data.InteractionTypes;
using xApi.Data.Json;

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
            ActivityDefinition activityDefinition = null;
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
                        activityDefinition = new ActivityDefinition();
                        if (!reader.IsDBNull(6))
                        {
                            interactionIndex = reader.GetInt32(6);
                        }
                        if (!reader.IsDBNull(1))
                        {
                      activityDefinition.Name = new LanguageMap(reader.GetString(1));
                            
                        }
                        if (!reader.IsDBNull(2))
                        {
                            activityDefinition.Description = new LanguageMap(reader.GetString(2));
                        }
                        if (!reader.IsDBNull(3))
                        {
                            activityDefinition.Type = new Iri(reader.GetString(3));
                        }
                        if (!reader.IsDBNull(4))
                        {
                            activityDefinition.MoreInfo = new Uri(reader.GetString(4));
                        }
                        if (!reader.IsDBNull(5))
                        {
                            activityDefinition.Extensions = new ExtensionsDictionary(reader.GetString(5));
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
                var temp = activityDefinition;
             var interactionDef = GetActivityInteraction(interactionIndex);
                activityDefinition = interactionDef;
                activityDefinition.Type = temp.Type;
                activityDefinition.MoreInfo = temp.MoreInfo;
                activityDefinition.Description = temp.Description;
                activityDefinition.Extensions = temp.Extensions;
                activityDefinition.Name = temp.Name;
            }
            return activityDefinition;
        }

        private ActivityDefinition GetActivityInteraction(int interactionIndex)
        {
            ActivityDefinition interactionDef = null;
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
                        InteractionType type = reader.GetString(1);
                        interactionDef = type.CreateInstance("", ApiVersion.GetLatest());
                        if (!reader.IsDBNull(2))
                        {
                            JsonString json = reader.GetString(2);
                            var sList = new List<string>();
                            var crp = json.ToJArray();
                            foreach(var jstring in crp)
                            {
                                sList.Add(jstring.Value<string>());
                            }
                        (interactionDef as InteractionActivityDefinitionBase).CorrectResponsesPattern = sList.ToArray();
                        }
                        if (!reader.IsDBNull(3))
                        {
                            Choice cho = interactionDef as Choice;
                            Sequencing seq = interactionDef as Sequencing;
                            if (cho != null)
                            {
                                cho.Choices = new InteractionComponentCollection(reader.GetString(3), ApiVersion.GetLatest());
                            } else if (seq != null)
                            {
                                seq.Choices = new InteractionComponentCollection(reader.GetString(3), ApiVersion.GetLatest());
                            }

                        }
                        if (!reader.IsDBNull(4))
                        {
                            Likert def = interactionDef as Likert;
                            if (def != null)
                            {
                                def.Scale = new InteractionComponentCollection(reader.GetString(4), ApiVersion.GetLatest());
                            }
                        }
                        if (!reader.IsDBNull(5))
                        {
                            Matching match = interactionDef as Matching;
                            if (match != null)
                            {
                                match.Source = new InteractionComponentCollection(reader.GetString(5), ApiVersion.GetLatest());
                            }
                        }
                        if (!reader.IsDBNull(6))
                        {
                            Matching match = interactionDef as Matching;
                            if (match != null)
                            {
                                match.Target = new InteractionComponentCollection(reader.GetString(6), ApiVersion.GetLatest());
                            }
                        }
                        if (!reader.IsDBNull(7))
                        {
                            Performance perf = interactionDef as Performance;
                                if (perf != null)
                            {
                                perf.Steps = new InteractionComponentCollection(reader.GetString(7), ApiVersion.GetLatest());
                            }
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
            return interactionDef;
        }
    }
}