using GL.Common;
using GL.DBOptions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.ApiServer.Filter
{

    //config.Filters.Add(new AuthenticationAttribute());
    //[Authentication]
    //public class ApiControllerBase : ApiController
    //{
    //}

    public class AuthenticationAttribute : AbsBaseAuthenticationAttribute
    {

        //TODO:从缓存中或者数据库 读取
        protected override string GetPartnerSecret(string partnerKey)
        {
            string secret = string.Empty;
            var ObjSecret = HelperMemoryCacher.Instance.GetValue("APP_PARTNERKEY_" + partnerKey);
            if (null == ObjSecret || string.IsNullOrEmpty(ObjSecret.ToString()))
            {
                secret = new AppService().GetSecretByKey(partnerKey); 
                HelperMemoryCacher.Instance.SetValue("APP_PARTNERKEY_" + partnerKey, secret, 1000 * 60 * 10);
            }
            else
            {
                secret = ObjSecret.ToString();
            } 
            return secret;
        }
    }
}
