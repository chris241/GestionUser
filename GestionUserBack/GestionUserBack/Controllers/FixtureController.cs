using GestionUserBack.Utility.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GestionUserBack.Controllers
{
    [RoutePrefix("api/dev/fixture")]
    public class FixtureController : ApiController
    {
        public FixtureController()
        {

        }
        [HttpGet]
        [Route("all")]
        public HttpResponseMessage Fixture()
        {
             DataFixtureHelper.GetInstance().CreateUser();
            return Request.CreateResponse(HttpStatusCode.OK, "Informations Enregistrées");

        }
    }
}