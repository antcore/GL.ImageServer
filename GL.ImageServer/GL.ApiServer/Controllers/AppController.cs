using GL.Common;
using GL.DBOptions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Newtonsoft.Json;
using GL.DBOptions.Models;

namespace GL.ApiServer.Controllers
{
    public class AppController : Controller
    {
        AppService appService = new AppService();
        // GET: App
        public ActionResult List()
        {
            var member = HelperSession.Instance.GetMemberInfo();
            if (null == member)
            {
                return Redirect("/User/login");
            }
            //获取 会员所有创建应用

            var apps = appService.GetAll(member.memberId);

            ViewBag.Apps = JsonConvert.SerializeObject(apps);
            ViewBag.memberName = member.memberUserName;

            return View();
        }

        public JsonResult Create(string sAppName)
        {
            var member = HelperSession.Instance.GetMemberInfo();
            GL_APP_Authorized model = new GL_APP_Authorized
            {
                sAppName = sAppName,
                sMemberId = member.memberId,
            };
            var result = appService.Insert(model); 
            return Json(result);
        }

    }
}