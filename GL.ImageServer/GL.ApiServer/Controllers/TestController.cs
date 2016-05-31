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
        

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public long GetTimeStampNum()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1999, 01, 01, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        public string GetSign(Dictionary<string, string> parameters, string secret)
        { 
            var sortedParams = new SortedDictionary<string, string>(parameters);

            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            StringBuilder query = new StringBuilder(secret);
            string _key = string.Empty;
            string _value = string.Empty;
            while (dem.MoveNext())
            {
                _key = dem.Current.Key;
                _value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key))
                {
                    query.Append(_key).Append(_value);
                }
            }
            query.Append(secret);

            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));

            StringBuilder result = new StringBuilder();
            long bytesLength = bytes.Length;
            for (int i = 0; i < bytesLength; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }
            return result.ToString();
        }

        string key = "3C09A1BA8456951184148F2C284AC24A";
        string Secret = "5B53FC659A78E345";

        // GET: Test
        public ActionResult Index()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            string timestamp = GetTimeStampNum().ToString();
       
            param.Add("timestamp", timestamp);
            param.Add("dirId", "");

            string sign = GetSign(param, Secret);

            ViewBag.key = key;
            ViewBag.timestamp = timestamp;
            ViewBag.sign = sign;


            return View();
        }


    }
}