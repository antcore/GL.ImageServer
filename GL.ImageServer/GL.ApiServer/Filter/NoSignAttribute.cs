using GL.Common;
using GL.DBOptions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.ApiServer.Filter
{
    /// <summary>
    /// 使用验证新 [NoSign] 标注不需要签名验证
    /// </summary>
    /// 
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class NoSignAttribute : Attribute
    {
    }


    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
    public class SignOnlyUriAttribute : Attribute
    {

    }

}
