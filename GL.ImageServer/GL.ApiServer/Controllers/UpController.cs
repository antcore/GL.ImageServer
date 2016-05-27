using GL.Common;
using GL.DBOptions.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CloudServer.API.Controllers
{
    public class FileResultInfo
    {
        /// <summary>
        /// 访问文件唯一标识
        /// </summary>
        public string guid { get; set; }
        /// 获取文件
        /// </summary>
        public string getFile { get; set; }

    }

    /// <summary>
    /// 文件上传 处理 API
    /// </summary>
    public class UpController : ApiController
    {
        /// <summary>
        /// 文件上传处理接口 -- 接收表单[multipart/form-data]提交文件 处理接口
        /// </summary>
        /// <param name="key">访问授权 KEY</param>
        /// <param name="source">访问来源 </param>
        /// <param name="dirId">存储 目录</param>
        /// <returns>用户访问 获取文件路径 等</returns>
        [Route("api/up/{key}/{Source}/{dirId}")]
        public async Task<HttpResponseMessage> Post(string key, string source, string dirId)
        {
            // multipart/form-data
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            //文件信息
            GL_Images img = null;
            List<GL_Images> listFiles = new List<GL_Images>();
            //返回客户端信息
            FileResultInfo resultModel = null;
            List<FileResultInfo> returnInfo = new List<FileResultInfo>();

            string filenames;
            string contentType;
            Stream ms;
            byte[] data;
            FileInfo info = null;

            ResultMessage resultInfo = new ResultMessage();
            try
            {
                foreach (var item in provider.Contents)
                {
                    if (item.Headers.ContentDisposition.FileName != null)
                    {
                        filenames = (item.Headers.ContentDisposition.FileName).Replace("\"", "");
                        contentType = MimeMapping.GetMimeMapping(filenames);
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
                                iSource = int.Parse(source), //文件来源

                                 sFilePath =""


                                FilePath =
                                    string.Format(@"/{0}/{1}/{2}/", ServerSettingInfo.ServerPath,
                                        DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString()),
                                DirId = dirId,
                                Source =
                                CreationTime = DateTime.Now,
                                IPUrl = ServerSettingInfo.ServerURL
                                //UserKey = key
                            };
                            //Write File

                            // 判断 路径存在?
                            if (!Directory.Exists(ServerSettingInfo.SaveDisc + fileModel.FilePath))
                            {
                                Directory.CreateDirectory(ServerSettingInfo.SaveDisc + fileModel.FilePath);
                            }
                            //补全 文件路径
                            fileModel.FilePath = string.Format(@"{0}{1}.{2}", fileModel.FilePath, fileModel.GuidValue, fileModel.FileSiffix);
                            //写入磁盘
                            File.WriteAllBytes(ServerSettingInfo.SaveDisc + fileModel.FilePath, data);

                            #region 上传文件 是 图片

                            if (contentType.IndexOf("image") != -1)
                            {
                                fileModel.FileUrl = string.Format(@"/api/down/img/{0}", fileModel.GuidValue);
                            }
                            else
                            {
                                //获取文件路径 --备用
                                fileModel.FileUrl = string.Format(@"/api/down/file/{0}", fileModel.GuidValue);
                            }

                            #endregion 上传文件 是 图片

                            listFiles.Add(fileModel);

                            //获取文件请求路径  并需要返回至客户端
                            resultModel = new FileResultInfo
                            {
                                guid = fileModel.GuidValue,
                                //getPath = string.Format(@"{0}{1}", fileModel.IPUrl, fileModel.ImgUrl),
                                getFile = string.Format(@"{0}{1}.{2}", fileModel.IPUrl, fileModel.FileUrl, fileModel.FileSiffix)
                            };

                            returnInfo.Add(resultModel);
                        }
                    }
                }
                if (returnInfo.Count > 0)
                {          //保存至数据库--
                    FileService.Instance.SaveModel(listFiles);

                    resultInfo.success = true;
                    resultInfo.message = returnInfo;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Instance.Wirtelog(ex);
                resultInfo.message = returnInfo;
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(resultInfo), System.Text.Encoding.UTF8, "application/json")
            };
        }

        private class ResultMessage
        {
            public bool success { get; set; }
            public object message { get; set; }

            public ResultMessage()
            {
                success = false;
                message = "上传失败！";
            }
        }
    }
}