using GL.DBOptions.Models;
using GL.DBOptions.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace GL.ApiServer.Controllers
{
    public class DirectoryController : ApiController
    {
        // GET: api/Directory
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Directory/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Directory
        public IHttpActionResult Post([FromBody]GL_Directory model)
        {
            return Json(new DirectoryService().Insert(model));
        }

        // PUT: api/Directory/5
        public IHttpActionResult Put(string id, [FromBody]GL_Directory model)
        {
            return Json(new DirectoryService().Update(id, model));
        }

        // DELETE: api/Directory/5
        public void Delete(int id)
        {
        }
    }
}