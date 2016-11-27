using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace GL.ImgServer.Client
{
    internal class HelperSecurity
    {
        public static string GetSecuritySign(string partnerSecret, out string timestamp, Dictionary<string, string> param)
        {
            timestamp = GetTimeStamp().ToString();
            if (null == param)
            {
                param = new Dictionary<string, string>();
            }
            param.Add("timestamp", timestamp);
            string sign = GetSign(param, partnerSecret);
            param.Remove("timestamp");
            return sign;
        }

        private static string GetSign(Dictionary<string, string> parameters, string secret)
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
                if (!string.IsNullOrEmpty(_key))
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

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        private static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1999, 01, 01, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }



    }
}
