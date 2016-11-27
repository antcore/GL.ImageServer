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

namespace GL.Common
{
    public class HelperSecurity
    {

        public static string GetSecuritySign(string partnerSecret, string timestamp, NameValueCollection postCollection)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("timestamp", timestamp);
            string _key = string.Empty;
            if (null != postCollection && postCollection.Count > 0)
            {
                for (int i = 0; i < postCollection.Count; i++)
                {
                    _key = postCollection.Keys[i];
                    param.Add(_key, postCollection[_key]);
                }
            }
            return GetSign(param, partnerSecret);
        }

        public static string GetSign(Dictionary<string, string> parameters, string secret)
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
        /// 验证时间戳是否有效
        /// </summary>
        /// <returns></returns>
        public static bool VilidateTimestamp(string timestamp)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(timestamp) && Regex.IsMatch(timestamp, "^[0-9]+$"))
            {
                long start = GetTimeStampNum();
                long end = long.Parse(timestamp);
                if ((start - end) < (1000 * 60 * 10))//10分钟内
                {
                    flag = true;
                }
            }
            return flag;
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStampNum()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1999, 01, 01, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }


    }
}
