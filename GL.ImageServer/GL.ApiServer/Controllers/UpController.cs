using GL.Common;
using GL.DBOptions.Models;
using GL.DBOptions.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GL.ApiServer.Controllers
{
    /// <summary>
    /// 文件上传 处理 API
    /// </summary>
    public class UpController : ApiController
    { 
        /// <summary>
        /// 文件上传处理接口 -- 接收表单[multipart/form-data]提交文件 处理接口
        /// </summary>
        /// <param name="dirId">存储目录</param> 
        /// <param name="key">访问授权 KEY</param>
        /// <returns>获取图片访问路径</returns>
        [Route("api/up/{key}/{dirId}")]
        public async Task<HttpResponseMessage> Post(string dirId,string key)
        {
            HelperResultMsg result = new HelperResultMsg(); 

            // multipart/form-data
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

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
                                iSource = 1, //文件来源
                                //
                                sDirId = dirId,
                                //sUriDomain = ServerSetting.sServerUriDomain,
                                //sUriPath = string.Empty,
                                //sFilePath = string.Empty,
                                dCreateTime = DateTime.Now
                            };
                            // 盘符或者服务网站根目录 + 存储文件夹 + 年月动态文件夹
                            img.sFilePath = string.Format(@"/{0}/{1}/{2}", ServerSetting.SaveDisc, ServerSetting.ServerPath, img.dCreateTime.ToString("yyyyMM"));
                            //Write File

                            //判断文件路径
                            if (!Directory.Exists(img.sFilePath))
                            {
                                Directory.CreateDirectory(img.sFilePath);
                            }
                            //文件存储文件路径
                            img.sFilePath = string.Format(@"{0}{1}.{2}", img.sFilePath, img.sId, img.sFileSiffix);
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
                HelperNLog.Default.Error("[图片服务器]上传异常", ex);
                result.message = "上传出现异常";
            }
            return new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(result), System.Text.Encoding.UTF8, "application/json")
            };
        }
    }
}