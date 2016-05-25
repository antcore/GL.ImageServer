using System;
using System.Configuration;
using System.Diagnostics;

namespace GL.Common
{
    public class HelperReadConfig
    {
        /// <summary>
        /// 读取配置文件中的节点值
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public static string ReadAppSetting(string appName)
        {
            string result = string.Empty;
            try
            {
                result = ConfigurationManager.AppSettings[appName];
            }
            catch (Exception e)
            {
                EventLog.WriteEntry("EstateErp.Web", "读取配置文件失败:" + e.ToString());
            }
            return result;
        }
    }
}