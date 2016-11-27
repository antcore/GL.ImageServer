using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GL.ImgServer.Client
{


    public class HelperImageServer
    {

        #region 类句柄操作

        private IntPtr handle;
        private bool disposed = false;

        //关闭句柄
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public extern static Boolean CloseHandle(IntPtr handle);

        /// <summary>
        /// 设置或获取Device的对像
        /// </summary>
        private static HelperImageServer instance;

        public static HelperImageServer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HelperImageServer();
                }
                return instance;
            }
            set
            {
                instance = value;
            }
        }

        /// <summary>
        /// 释放对象资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放对象资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (handle != IntPtr.Zero)
                {
                    CloseHandle(handle);
                    handle = IntPtr.Zero;
                }
            }
            disposed = true;
        }

        ~HelperImageServer()
        {
            Dispose(false);
        }

        public HelperImageServer()
        {
        }

        #endregion 类句柄操作


        /************************ 


         web.config 配置参数


         <add key="IMAGESERVER:host" value="http://localhost:4001"/> 
         <add key="IMAGESERVER:Appid" value="F4D12775BCE2F3D7"/> 
         <add key="IMAGESERVER:Key" value="7536B6557CDC7FFA"/> 
         <add key="IMAGESERVER:Secret" value="F3AC1F757536B6557CDC7FFAEAD3B649"/> 


         ************************/


        private static string Host { get; set; } = ConfigurationManager.AppSettings["IMAGESERVER:Host"];
        private static string Appid { get; set; } = ConfigurationManager.AppSettings["IMAGESERVER:Appid"];
        private static string Key { get; set; } = ConfigurationManager.AppSettings["IMAGESERVER:Key"];
        private static string Secret { get; set; } = ConfigurationManager.AppSettings["IMAGESERVER:Secret"];


        private const string WEBURI = "{0}/api/{1}/webup?key={2}&sign={3}&timestamp={4}";
        private const string WECHATURI = "{0}/api/{1}/wechatup?key={2}&sign={3}&timestamp={4}";
        private const string BASE64URI = "{0}/api/{1}/base64up?key={2}&sign={3}&timestamp={4}";


        public string GetUri(AddressType address)
        {
            string timeStamp = string.Empty;

            var sign = HelperSecurity.GetSecuritySign(Secret, out timeStamp, null);

            string uri = string.Empty;

            switch (address)
            {
                case AddressType.WEBUP:
                    uri = string.Format(WEBURI, Host, Appid, Key, sign, timeStamp);
                    break;
                case AddressType.WECHAT:
                    uri = string.Format(WECHATURI, Host, Appid, Key, sign, timeStamp);
                    break;
                case AddressType.BASE64:
                    uri = string.Format(BASE64URI, Host, Appid, Key, sign, timeStamp);
                    break;
                default:
                    break;
            }
            return uri;
        }

        /// <summary>
        /// 客户端模拟  MultipartFormData 上传至图片服务器
        /// </summary>
        /// <param name="list">文件二进制数据</param>
        /// <returns></returns>
        public async Task<string> ClientMultipartFormDataWebUp(List<ByteArrayContent> list)
        {
            string uri = GetUri(AddressType.WEBUP);

            string result = string.Empty;

            using (HttpClient client = new HttpClient())
            {
                //设定要响应的数据格式  text/json或 text/xml 
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));

                //通过multipart/form-data的方式上传数据
                using (var content = new MultipartFormDataContent())
                {
                    //声明一个委托，该委托的作用就是将ByteArrayContent集合加入到MultipartFormDataContent中
                    Action<List<ByteArrayContent>> act = (dataContents) =>
                    {
                        foreach (var byteArrayContent in dataContents)
                        {
                            content.Add(byteArrayContent);
                        }
                    };
                    act(list);
                    try
                    {//post请求
                        var response = await client.PostAsync(uri, content);

                        result = await response.Content.ReadAsStringAsync();
                    }
                    catch (Exception ex)
                    {
                        result = ex.ToString();//将异常信息显示在文本框内
                    }
                }
            }

            return result;
        }




    }
}
