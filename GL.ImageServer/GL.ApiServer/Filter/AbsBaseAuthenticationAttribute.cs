using System;
using System.Web;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// WebAPI防篡改签名验证抽象基类Attribute
/// </summary>
public abstract class AbsBaseAuthenticationAttribute : ActionFilterAttribute
{
    /// <summary>
    /// Occurs before the action method is invoked.
    /// </summary>
    /// <param name="actionContext">The action context</param>
    public override void OnActionExecuting(HttpActionContext actionContext)
    {
        //获取Asp.Net对应的Request
        var request = ((HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request;
        NameValueCollection getCollection = request.QueryString;//此签名要求Partner及Sign均通过QueryString传递
        if (getCollection != null && getCollection.Count > 0)
        {
            string partnerkey = getCollection["key"];
            string sign = getCollection["sign"];
            string timestamp = getCollection["timestamp"];
            if (
               VilidateTimestamp(timestamp)
               && !string.IsNullOrWhiteSpace(partnerkey)//partnerkey
               && !string.IsNullOrWhiteSpace(sign)//必须包含sign
               && Regex.IsMatch(sign, "^[0-9A-Za-z]{32}$")
               )//sign必须为32位Md5摘要
            {
                //获取partner对应的key
                //这里暂时只做了合作key校验，不做访问权限校验，如有需要，此处可进行调整，建议RBAC
                string partnerSecret = this.GetPartnerSecret(partnerkey);
                if (!string.IsNullOrWhiteSpace(partnerSecret))
                {
                    NameValueCollection postCollection = null;
                    switch (request.RequestType.ToUpper())
                    {
                        case "GET":
                        case "DELETE": break;//只是为了同时显示restful四种方式才有这部分无意义代码
                        //实际该以哪种方式进行请求应遵循restful标准
                        case "POST":
                        case "PUT":
                            postCollection = request.Form;//post的数据必须通过application/x-www-form-urlencoded方式传递
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    //根据请求数据获取MD5签名
                    string vSign = GetSecuritySign(partnerkey, partnerSecret, timestamp, postCollection);
                    if (string.Equals(sign, vSign, StringComparison.OrdinalIgnoreCase))
                    {//验证通过,执行基类方法
                        base.OnActionExecuting(actionContext);
                        return;
                    }
                    else
                    {
                         
                    }
                }
            }
        }
        //此处暂时以401返回，可调整为其它返回
        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
    }

    private string GetSecuritySign(string partnerkey, string partnerSecret, string timestamp, NameValueCollection postCollection)
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("timestamp", timestamp);
        string _key = string.Empty;
        if (null!= postCollection && postCollection.Count>0) {
            for (int i = 0; i < postCollection.Count; i++)
            {
                _key = postCollection.Keys[i];
                param.Add(_key, postCollection[_key]);
            }
        }
        return GetSign(param, partnerSecret);
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
    /// 获取时间戳计算
    /// </summary>
    /// <returns></returns>
    public bool VilidateTimestamp(string timestamp)
    {
        bool flag = false;
        if (!string.IsNullOrEmpty(timestamp))
        {
            long end = 0;
            if (Regex.IsMatch(timestamp, "^[0-9]+$"))
            {
                end = long.Parse(timestamp);
            }
            long start = GetTimeStampNum();
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
    public long GetTimeStampNum()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1999, 01, 01, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalMilliseconds);
    }

    /// <summary>
    /// 获取合作号对应的合作Key,如果未能获取，则返回空字符串或者null
    /// </summary>
    /// <param name="partner"></param>
    /// <returns></returns>
    protected abstract string GetPartnerSecret(string partner);
}