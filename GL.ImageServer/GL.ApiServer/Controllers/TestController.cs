using GL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GL.ApiServer.Controllers
{
    public class TestController : Controller
    {

        string key = "3C09A1BA8456951184148F2C284AC24A";
        string Secret = "5B53FC659A78E345";

        string appid = "730117a69d4520ab23e338a7734aa8ca";
        //string key = "550085CD4490EBBC2B482F8811322452";
        //string Secret = "5D63A562B0120AA7";

        //string memberId = "f40017a6a909f65685981240cd40bb77";
        // GET: Test
        public ActionResult Index()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            string timestamp = HelperSecurity.GetTimeStampNum().ToString();

            param.Add("timestamp", timestamp);
            //param.Add("dirId", "");

            string sign = HelperSecurity.GetSign(param, Secret); 
            ViewBag.key = key;
            ViewBag.appid = appid;
            ViewBag.timestamp = timestamp;
            ViewBag.sign = sign;

            //param.Clear();
            //timestamp = HelperSecurity.GetTimeStampNum().ToString();
            //param.Add("timestamp", timestamp);
            //param.Add("sDirName", "444");
            //sign = HelperSecurity.GetSign(param, Secret);
            //ViewBag.key1 = key;
            //ViewBag.timestamp1 = timestamp;
            //ViewBag.sign1 = sign;


            return View();
        }


    }
}