using GL.DBOptions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;

namespace GL.ApiServer.Controllers
{
    public class ImageController : ApiController
    {

        // POST: api/Image/List
        public IHttpActionResult List([FromBody]JObject data)
        {
            string appId = GetValue(data, "appId"); 
            string pageSize = GetValue(data, "pageSize");
            string pageIndex = GetValue(data, "pageIndex"); 
            return Json(new ImageService().GetByAppId(appId, int.Parse(pageSize), int.Parse(pageIndex)));
        }
        public   string GetValue(JToken jNode, string sAttrName)
        {
            string sResult = string.Empty;
            try
            {
                sResult = ((JValue)jNode[sAttrName]).Value.ToString();
            }
            catch
            {
            }
            return sResult;
        }

    }
}
