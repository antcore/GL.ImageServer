using GL.ImgServer.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;


using System.Threading.Tasks;
using System.Text;

namespace Gl.ImgServer.Client.Test.Web.Controllers
{
    public class HomeController : Controller
    {


        // GET: Home
        public ActionResult Index()
        {

            ViewBag.ImageServerUri = HelperImageServer.Instance.GetUri(AddressType.WEBUP);

            return View();
        }




        public async Task<ActionResult> ClientWebUp()
        {
            string result = string.Empty;

            List<ByteArrayContent> list = new List<ByteArrayContent>();


            #region 上传文件 转ByteArrayContent 支持多文件
            string FileName = string.Empty;
            byte[] bytes = new byte[12];

            var fileContent = new ByteArrayContent(bytes.ToArray());
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = FileName
            };
            list.Add(fileContent);


            #endregion


            result = await HelperImageServer.Instance.ClientMultipartFormDataWebUp(list);
            return Content(result);
        }




        //public async Task<JsonResult> Post()
        //{
        //    StringBuilder info = new StringBuilder();
        //    foreach (string file in Request.Files)
        //    {
        //        HttpPostedFileBase postFile = Request.Files[file];//get post file  

        //        if (postFile.ContentLength == 0)
        //            continue;

        //        string newFilePath = @"D:/";//save path 
        //        postFile.SaveAs(newFilePath + Path.GetFileName(postFile.FileName));


        //        info.AppendFormat("Upload File:{0}/r/n", postFile.FileName);//info 
        //    }
        //}


    }
}