using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Common
{
    /// <summary>
    /// 当前服务器 配置
    /// </summary>
    public class ServerSetting
    {
        /// <summary>
        /// 数据库配置中图片服务器地址 域名或者IP访问[http://IP:PORT]
        /// </summary>
        public static string ServerURL { get; set; }  
        /// <summary>
        /// 数据库配置中图片服务器 图片存储 磁盘盘符
        /// </summary>
        public static string SaveDisc { get; set; }
        /// <summary>
        /// 数据库配置中图片服务器 图片存储路径
        /// </summary>
        public static string ServerPath { get; set; }
        /// <summary>
        /// web.config 中配置参数 ServerCode 值  等同
        /// 数据库配置图片存储路径一致
        /// </summary>
        public static string ServerCode { get; set; }
    }


}
