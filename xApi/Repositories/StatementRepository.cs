using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using xApi.ApiUtils;
using xApi.Data;
using xApi.Data.Helpers;
using xApi.Domain;

namespace xApi.Repositories
{

    public class StatementRepository

    {
        private const string GetStatementId = "SELECT TOP 1 id from dbo.Statement "
                + "WHERE id = @statementId ;";
        private const string GetStatementVoid = "SELECT TOP 1 is_voided, is_voiding from dbo.Statement "
                + "WHERE id = @id ;";

        public bool PutStatement(Guid? statementId, Statement statement, out HttpStatusCode statusCode)
        {
            statusCode = HttpStatusCode.Conflict;
            statement.Id = statementId;

            if (statement.Id.HasValue)
            {
                if(CheckIfExists(statementId)) { return true; }
            }

            if(statement.Verb.Id == KnownVerbs.Voided)
            {
                StatementRef stRf = statement.Object as StatementRef;
                if(IsVoidedOrVoiding(stRf.Id))
                {
                     return true;
                }
            }

            statement.Stamp();
            // Ensure statement version and stored date
            statement.Version = statement.Version ?? ApiVersion.GetLatest().ToString();
            statement.Stored = statement.Stored ?? DateTimeOffset.UtcNow;

            if (statement.Authority == null)
            {
                // Set authority before saving JSON encoded statement
                var agent = new Agent();
                agent.Mbox = new Mbox("mailto:" + "lucaciemanueladrian@gmail.com");
                agent.Name = "Manu";
                statement.Authority = agent;
            }


            //to do map to Statement Entity 
            StatementEntity newStatement = new StatementEntity();
            newStatement.Id = statement.Id;
            newStatement.Stored = statement.Stored;
            newStatement.Version = statement.Version.ToString();
            newStatement.FullStatement = statement.ToJson();

            StoreStatement(newStatement);


            return false;
        }

        private bool IsVoidedOrVoiding(Guid id)
        {
            bool isVoidedOrVoiding = false;

            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                SqlDataReader reader = null;
                try
                {
                    // get statement
                    SqlCommand command = new SqlCommand(GetStatementVoid, connection);
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                       if(reader.GetBoolean(0) || reader.GetBoolean(1))
                        {
                            isVoidedOrVoiding = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return isVoidedOrVoiding;
                }
                finally
                {
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }
                }
            }


                return isVoidedOrVoiding;
        }

        public void StoreStatement(StatementEntity newStatement)
        {
    
        }

        public bool CheckIfExists(Guid? statementId)
        {
            bool alreadyExists = false;

            using (SqlConnection connection = new SqlConnection(DbUtils.GetConnectionString()))
            {
                SqlDataReader reader = null;
                try
                {
                    // get statement
                    SqlCommand command = new SqlCommand(GetStatementId, connection);
                    command.Parameters.AddWithValue("@statementId", statementId);
                    connection.Open();
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        alreadyExists = true;
                    }

                }
                catch (Exception ex)
                {
                    return alreadyExists;
                }
                finally
                {
                    if (reader != null && !reader.IsClosed)
                    {
                        reader.Close();
                    }
                }


                return alreadyExists;
            }
        }
    }
}