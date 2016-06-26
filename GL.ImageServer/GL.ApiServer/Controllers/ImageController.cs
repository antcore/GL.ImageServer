using GL.DBOptions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GL.ApiServer.Controllers
{
    public class ImageController : ApiController
    {

        // POST: api/Image
        public IHttpActionResult Get(string appId, int pageSize, int pageIndex)
        {
            return Json(new ImageService().GetByAppId(appId, pageSize, pageIndex));
        }


    }
}
