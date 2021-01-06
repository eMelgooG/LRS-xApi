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

        [HttpPut]
        public IHttpActionResult PutStatement([FromUri] Guid statementId, Statement statement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

             _statementRepository.PutStatement(statementId, statement);

            return StatusCode(HttpStatusCode.NoContent);
        }

    }
}