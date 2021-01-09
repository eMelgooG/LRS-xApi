using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using xApi.Data;
using xApi.Repositories;

namespace xApi.Controllers
{

    [Route("xapi/statements")]
    public class StatementsController : ApiController
    {
        StatementRepository _statementRepository;
        public StatementsController()
        {
            _statementRepository = new StatementRepository();
        }


        //create a RequestModel that encapsulates both the statement and the statementId, so you don't deserialize the Statement before checking if the statementId is null
        [HttpPut]
        public IHttpActionResult PutStatement(Statement statement,[FromUri] Guid? statementId=null)
        {
            if(statementId == null)
            {
                return BadRequest("You need to provide a valid Guid as a statementId");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(statement.Id!=null && statement.Id!=statementId)
            {
                return BadRequest("Where provided, the 'id' property of the Statement MUST match the 'statementId' parameter of the request");
            }

            if(_statementRepository.PutStatement(statementId, statement, out var statusCode)){
                return StatusCode(statusCode);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

    }
}