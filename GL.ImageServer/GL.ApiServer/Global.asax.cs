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
            ServerSetting.ServerCode = HelperReadConfig.ReadAppSetting("IMAGE_SERVERCODE");
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
                HelperNLog.Default.Error("读取服务器-配置错误" + DateTime.Now.ToShortDateString());
            }
            #endregion




            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }
    }
}