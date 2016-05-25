using GL.Common;
using GL.DBOptions.Models;
using GL.DBOptions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GL.ApiServer.Controllers
{
    public class UserController : Controller
    {


        // GET: User
        public ActionResult Reg()
        {
            return View();
        }

        public JsonResult SubmitReg(GL_Member model)
        {
            return Json(new MemberService().Insert(model));
        }

        public ActionResult Login()
        {
            return View();
        }

        public JsonResult SubmitLogin(string sUserName, string sUserPwd)
        {
            var result = new MemberService().Login(sUserName, sUserPwd);
            HelperSession.Instance.SetMemberInfo(result.resultInfo as MemberSessionModel);
            return Json(result);
        }





    }
}