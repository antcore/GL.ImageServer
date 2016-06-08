using GL.ApiServer.Filter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GL.ApiServer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new AuthenticationAttribute());

            // Web API 配置和服务
            var allowedMethods = ConfigurationManager.AppSettings["cors:allowedMethods"];
            var allowedOrigin = ConfigurationManager.AppSettings["cors:allowedOrigin"];
            var allowedHeaders = ConfigurationManager.AppSettings["cors:allowedHeaders"];

            var autoCors = new EnableCorsAttribute(allowedOrigin, allowedHeaders, allowedMethods)
            {
                SupportsCredentials = true
            };
            //跨域配置
            config.EnableCors(autoCors);



            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


        }
    }
}
