using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data;
using xApi.Data.Helpers;

namespace xApi.Repositories
{
    public class StatementRepository
    {
        public void PutStatement(Guid? statementId, Statement statement)
        {
            statement.Id = statementId;
            
            if (statement.Id.HasValue)
            {
                //check if the Guid of the statement already exists
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
            var id = statement.Id;
        }
    }
}