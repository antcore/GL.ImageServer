using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;

using GL.DBOptions.Services;
using GL.Common;
using GL.DBOptions.Models;

namespace GL.ApiServer
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            ServerSetting.ServerCode = HelperReadConfig.ReadAppSetting("setting:IMAGE_SERVER_CODE");

            ServerSetting.sVirtual_Directory = HelperReadConfig.ReadAppSetting("setting:Virtual_Directory");

            #region 获取 数据库中 图片服务器配置信息 根据图片服务器唯一识别 编码

            //初始化图片服务器信息
            GL_CloudSetting model = new SettingService().GetByServerCode(ServerSetting.ServerCode);
            if (model != null)
            {
                ServerSetting.ServerPath = model.sSavePath;
                ServerSetting.SaveDisc = model.sSaveDisc;
                ServerSetting.sServerUriDomain = model.sServerUriDomain;
            }
            else
            {
                ServerSetting.SaveDisc = HelperReadConfig.ReadAppSetting("setting:save_Disc");
                ServerSetting.ServerPath = HelperReadConfig.ReadAppSetting("setting:save_Path");

                ServerSetting.sServerUriDomain = HelperReadConfig.ReadAppSetting("setting:SERVER_URI_DOMAIN");

                HelperNLog.Default.Error("读取数据库图片服务器-配置错误-已读取文件web.config 配置" + DateTime.Now.ToShortDateString());
            }
            #endregion




            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}