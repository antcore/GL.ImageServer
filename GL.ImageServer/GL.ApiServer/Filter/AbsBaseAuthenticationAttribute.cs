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
using GL.Common;
using System.Threading.Tasks;
using System.Threading;
using GL.ApiServer.Filter;
using System.Linq;

namespace GL.ApiServer.Filter
{
    /// <summary>
    /// WebAPI防篡改签名验证抽象基类Attribute
    /// </summary>
    public abstract class AbsBaseAuthenticationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 判断类和方法头上的特性是否要进行Action拦截
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private static bool SkipNoSign(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<NoSignAttribute>().Any()
                || actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<NoSignAttribute>().Any();
        }

        private static bool SkipSignOnlyUri(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<SignOnlyUriAttribute>().Any()
                || actionContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<SignOnlyUriAttribute>().Any();
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (SkipNoSign(actionContext))//是否该类标记为NoSign
            {
                base.OnActionExecuting(actionContext);
                return;
            }
            //获取Asp.Net对应的Request
            var request = ((HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request;
            NameValueCollection getCollection = request.QueryString;//此签名要求 key sign timestamp  均通过QueryString传递
            if (getCollection != null && getCollection.Count > 0)
            {
                string partnerkey = getCollection["key"];
                string sign = getCollection["sign"];
                string timestamp = getCollection["timestamp"];
                if (
                   HelperSecurity.VilidateTimestamp(timestamp)//时间戳是否有效
                   && !string.IsNullOrWhiteSpace(partnerkey)//partnerkey
                   && !string.IsNullOrWhiteSpace(sign)//必须包含sign
                   && Regex.IsMatch(sign, "^[0-9A-Za-z]{32}$")//sign必须为32位Md5摘要
                   )
                {
                    //获取partner对应的key
                    //这里暂时只做了合作key校验，不做访问权限校验，如有需要，此处可进行调整 
                    string partnerSecret = this.GetPartnerSecret(partnerkey);
                    if (!string.IsNullOrWhiteSpace(partnerSecret))
                    {
                        NameValueCollection postCollection = null;
                        if (!SkipSignOnlyUri(actionContext))//判断 请求包含地址是否  仅验证Url  跳过内容加入签名验证
                        {
                            switch (request.RequestType.ToUpper())
                            {
                                case "GET":
                                case "DELETE":
                                    break;
                                case "POST":
                                case "PUT":
                                    postCollection = request.Form;//post的数据必须通过application/x-www-form-urlencoded 方式传递
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                        }
                        string vSign = HelperSecurity.GetSecuritySign(partnerSecret, timestamp, postCollection);

                        if (string.Equals(sign, vSign, StringComparison.OrdinalIgnoreCase))
                        {
                            //验证通过,执行基类方法
                            base.OnActionExecuting(actionContext);
                            return;
                        }

                    }
                }
            }
            //此处暂时以401返回，可调整为其它返回
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }
        /// <summary>
        /// 获取合作号对应的合作Key,如果未能获取，则返回空字符串或者null
        /// </summary>
        /// <param name="partnerKey"></param>
        /// <returns></returns>
        protected abstract string GetPartnerSecret(string partnerKey);


    }
}