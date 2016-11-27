using GL.ApiServer.Filter;
using GL.Common;
using GL.DBOptions.Models;
using GL.DBOptions.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace GL.ApiServer.Controllers
{
    /// <summary>
    /// 文件上传 处理 API
    /// </summary>

    [SignOnlyUri]
    public class UpController : ApiController
    {

        #region web 表单[multipart/form-data]提交文件

        /// <summary>
        /// 文件上传处理接口 -- 接收表单[multipart/form-data]提交文件 处理接口
        /// </summary>
        /// <param name="appId">应用ID</param> 
        /// <returns>获取图片访问路径</returns> 
        [Route("api/{appId}/webup")]
        public async Task<HttpResponseMessage> web(string appId)
        {
            HelperResultMsg result = new HelperResultMsg();
            if (string.IsNullOrEmpty(appId))
            {
                result.message = "不合法的 appid";
            }
            else
            {
                //文件信息
                GL_Images img = null;
                List<GL_Images> listFiles = new List<GL_Images>();
                //返回客户端信息
                List<string> resultString = new List<string>();
                string filenames;
                Stream ms;
                byte[] data;
                FileInfo info = null;
                try
                {
                    // multipart/form-data
                    var provider = new MultipartMemoryStreamProvider();
                    await Request.Content.ReadAsMultipartAsync(provider);

                    foreach (var item in provider.Contents)
                    {
                        if (item.Headers.ContentDisposition.FileName != null)
                        {
                            filenames = (item.Headers.ContentDisposition.FileName).Replace("\"", "");
                            //Strem
                            ms = item.ReadAsStreamAsync().Result;

                            using (var br = new BinaryReader(ms))
                            {
                                if (ms.Length <= 0)
                                    break;
                                data = br.ReadBytes((int)ms.Length);

                                //FileInfo
                                info = new FileInfo(item.Headers.ContentDisposition.FileName.Replace("\"", ""));

                                //Create
                                img = new GL_Images
                                {
                                    sId = HelperAutoGuid.Instance.GetGuidString(),
                                    sFileName = info.Name.Substring(0, info.Name.LastIndexOf(".")),
                                    sFileSiffix = info.Extension.Substring(1).ToLower(),
                                    iFileSize = ms.Length, //文件大小
                                    iSource = 1, //文件来源 web
                                                 //
                                    sAppId = appId,
                                    //sUriDomain = ServerSetting.sServerUriDomain,
                                    //sUriPath = string.Empty,
                                    //sFilePath = string.Empty,
                                    dCreateTime = DateTime.Now
                                };
                                // 盘符或者服务网站根目录 + 存储文件夹 + 年月动态文件夹
                                img.sFilePath = string.Format(@"{0}/{1}/{2}", ServerSetting.SaveDisc, ServerSetting.ServerPath, img.dCreateTime.ToString("yyyyMM"));
                                //Write File

                                //判断文件路径
                                if (!Directory.Exists(img.sFilePath))
                                {
                                    Directory.CreateDirectory(img.sFilePath);
                                }
                                //文件存储文件路径
                                img.sFilePath = string.Format(@"{0}/{1}.{2}", img.sFilePath, img.sId, img.sFileSiffix);
                                //写入磁盘
                                File.WriteAllBytes(img.sFilePath, data);

                                //获取文件路径--备用
                                img.sUriDomain = ServerSetting.sServerUriDomain;
                                img.sUriPath = string.Format(@"/api/down/{0}.{1}", img.sId, img.sFileSiffix);

                                listFiles.Add(img);

                                //获取文件请求路径
                                resultString.Add(string.Format(@"{0}{1}", img.sUriDomain, img.sUriPath));
                            }
                        }
                    }
                    if (listFiles.Count > 0)
                    {
                        //保存至数据库
                        var msg = new ImageService().InsertAll(listFiles);
                        if (msg.success)
                        {
                            result.success = true;
                            result.message = "上传成功";
                            result.resultInfo = resultString;
                        }
                    }
                }
                catch (Exception ex)
                {
                    HelperNLog.Default.Error("[图片服务器]表单[multipart/form-data]-上传异常", ex);
                    result.message = "表单[multipart/form-data]-上传出现异常";
                }
            }
            return new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(result), System.Text.Encoding.UTF8, "application/json")
            };
        }

        #endregion web 表单[multipart/form-data]提交文件


        #region 微信 传入 token与mediaid 下载图片

        public class WeChatUpdataModel
        {
            public string media_ids { get; set; }
            public string wechat_token { get; set; }
        }

        // POST: api/up/WeChat
        /// <summary>
        /// 下载微信服务器上图片
        /// </summary>
        /// <returns></returns>
        [Route("api/{appId}/wechatup")]
        public HttpResponseMessage WeChat(string appId, [FromBody]WeChatUpdataModel param)
        {
            HelperResultMsg result = new HelperResultMsg();
            string media_ids = param.media_ids;

            if (media_ids.Length <= 0)
            {
                result.message = "请微信上传图片 media_ids";
            }
            else
            {
                try
                {
                    GL_Images img;
                    List<string> resultString = new List<string>();
                    List<GL_Images> listFiles = new List<GL_Images>();
                    byte[] data;
                    string wechatUri = string.Empty;
                    HttpWebRequest webRequest; Stream ms; HttpWebResponse webResponse;
                    string[] MEDIA_IDS = media_ids.Split(',');
                    foreach (var media_id in MEDIA_IDS)
                    {
                        wechatUri = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", param.wechat_token, media_id);
                        webRequest = (HttpWebRequest)WebRequest.Create(wechatUri);
                        webRequest.ProtocolVersion = HttpVersion.Version10;
                        webRequest.Timeout = 30000;
                        webRequest.Method = "GET";
                        webRequest.UserAgent = "Mozilla/4.0";
                        webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                        webResponse = (HttpWebResponse)webRequest.GetResponse();
                        ms = webResponse.GetResponseStream();
                        using (var br = new BinaryReader(ms))
                        {
                            if (ms.Length <= 0)
                                break;
                            data = br.ReadBytes((int)ms.Length);
                            //Create
                            img = new GL_Images
                            {
                                sId = HelperAutoGuid.Instance.GetGuidString(),
                                sFileName = "微信上传图片",
                                sFileSiffix = "jpg",
                                iFileSize = ms.Length, //文件大小
                                iSource = 2, //文件来源 wechat
                                sAppId = appId,
                                dCreateTime = DateTime.Now
                            };
                            // 盘符或者服务网站根目录 + 存储文件夹 + 年月动态文件夹
                            img.sFilePath = string.Format(@"{0}/{1}/{2}", ServerSetting.SaveDisc, ServerSetting.ServerPath, img.dCreateTime.ToString("yyyyMM"));
                            //Write File
                            //判断文件路径
                            if (!Directory.Exists(img.sFilePath))
                            {
                                Directory.CreateDirectory(img.sFilePath);
                            }
                            //文件存储文件路径
                            img.sFilePath = string.Format(@"{0}/{1}.{2}", img.sFilePath, img.sId, img.sFileSiffix);
                            //写入磁盘
                            File.WriteAllBytes(img.sFilePath, data);
                            //获取文件路径
                            img.sUriDomain = ServerSetting.sServerUriDomain;
                            img.sUriPath = string.Format(@"/api/down/{0}.{1}", img.sId, img.sFileSiffix);
                            listFiles.Add(img);
                            //获取文件请求路径
                            resultString.Add(string.Format(@"{0}{1}", img.sUriDomain, img.sUriPath));
                        }
                    }
                }
                catch (Exception ex)
                {
                    HelperNLog.Default.Error("[图片服务器]wechat-上传异常", ex);
                    result.message = "wechat-上传出现异常";
                }
            }
            return new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(result), System.Text.Encoding.UTF8, "application/json")
            };
        }

        #endregion 微信 传入 token与mediaid 下载图片


        #region base64 上传图片

        public class Base64UpdataModel
        {
            /// <summary>
            /// 图片base64字符串 必须传人
            /// </summary>
            public string base64str { get; set; }


            /// <summary>
            /// 图片类型 文件后缀 jpg gif png ... 默认jpg
            /// </summary>
            public string fileSiffix { get; set; }

            /// <summary>
            /// 文件名称 默认 yyyyMMddHHmmssfff
            /// </summary>
            public string fileName { get; set; }
        }

        // POST: api/up/base64
        /// <summary>
        /// 图片base64字符串 上传文件图片
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Route("api/{appId}/base64up")]
        public HttpResponseMessage base64(string appid, [FromBody]Base64UpdataModel param)
        {
            HelperResultMsg result = new HelperResultMsg();

            if (string.IsNullOrEmpty(param.base64str))
            {
                result.message = "图片base64字符串未上传";
            }
            else
            {
                try
                {
                    GL_Images img;
                    List<string> resultString = new List<string>();
                    List<GL_Images> listFiles = new List<GL_Images>();
                    byte[] data = Convert.FromBase64String(param.base64str);
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        //Bitmap bmp = new Bitmap(ms);
                        img = new GL_Images
                        {
                            sId = HelperAutoGuid.Instance.GetGuidString(),
                            sFileName = (string.IsNullOrEmpty(param.fileName) ? DateTime.Now.ToString("yyyyMMddHHmmssfff") : param.fileSiffix),
                            sFileSiffix = (string.IsNullOrEmpty(param.fileSiffix) ? "jpg" : param.fileSiffix),
                            iFileSize = ms.Length, //文件大小

                            sAppId = appid,
                            dCreateTime = DateTime.Now
                        };
                        // 盘符或者服务网站根目录 + 存储文件夹 + 年月动态文件夹
                        img.sFilePath = string.Format(@"{0}/{1}/{2}", ServerSetting.SaveDisc, ServerSetting.ServerPath, img.dCreateTime.ToString("yyyyMM"));
                        //Write File
                        //判断文件路径
                        if (!Directory.Exists(img.sFilePath))
                        {
                            Directory.CreateDirectory(img.sFilePath);
                        }
                        //文件存储文件路径
                        img.sFilePath = string.Format(@"{0}/{1}.{2}", img.sFilePath, img.sId, img.sFileSiffix);
                        //写入磁盘
                        File.WriteAllBytes(img.sFilePath, data);

                        //获取文件路径
                        img.sUriDomain = ServerSetting.sServerUriDomain;
                        img.sUriPath = string.Format(@"/api/down/{0}.{1}", img.sId, img.sFileSiffix);
                        listFiles.Add(img);
                        //获取文件请求路径
                        resultString.Add(string.Format(@"{0}{1}", img.sUriDomain, img.sUriPath));

                        ms.Close();

                        result.success = true;
                        result.message = "上传成功";
                        result.resultInfo = new List<string>() { string.Format(@"{0}{1}", img.sUriDomain, img.sUriPath) };
                    }
                    if (listFiles.Count > 0)
                    {
                        //保存至数据库
                        var msg = new ImageService().InsertAll(listFiles);
                        if (msg.success)
                        {
                            result.success = true;
                            result.message = "上传成功";
                            result.resultInfo = resultString;
                        }
                    }
                }
                catch (Exception ex)
                {
                    HelperNLog.Default.Error("[图片服务器]base64-上传异常", ex);
                    result.message = "base64-上传出现异常";
                }
            }
            return new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json")
            };
        }

        #endregion base64 上传图片




    }
}