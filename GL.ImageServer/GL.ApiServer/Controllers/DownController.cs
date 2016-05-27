using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Common.Log;
using CloudServer.API.FileOption;

namespace CloudServer.API.Controllers
{
    /// <summary>
    /// 文件访问 处理 API
    /// </summary>
    [System.Web.Http.Cors.EnableCors("*", headers: "*", methods: "*")]
    public class apiDownController : ApiController
    {
        /// <summary>
        /// 获取缩略图 
        /// guidValue+water-width-height.jpg
        /// 图片ID+水印名称-宽度-高度 water[t/i/''] -->t/文本水印 i-图片水印 没有则不传值
        /// 例如：   
        /// 
        /// http://localhost:2000/api/down/06015fa557a7ff694a05c4c566468249+t.jpg --文本水印 原图
        /// http://localhost:2000/api/down/06015fa557a7ff694a05c4c566468249+i.jpg --png图片水印 原图  
        /// http://localhost:2000/api/down/06015fa557a7ff694a05c4c566468249.jpg   --无水印 原图 
        /// 
        /// http://localhost:2000/api/down/06015fa557a7ff694a05c4c566468249+t-1000-900.jpg --文本水印 1000*900
        /// http://localhost:2000/api/down/06015fa557a7ff694a05c4c566468249+i-1000-900.jpg --png图片水印 1000*900     
        /// http://localhost:2000/api/down/06015fa557a7ff694a05c4c566468249-1000-900.jpg   --无水印 1000*900 
        /// 
        /// </summary>
        /// <param name="fileInfo">guidValue+water-width-height.jpg ==》图片ID+水印名称-宽度-高度 water[t/i/''] -->t/文本水印 i-图片水印 没有则不传值</param> 
        [Route("api/down/img/{fileInfo}")]
        public async Task<HttpResponseMessage> GetImg(string fileInfo)
        {
            return await Task.Run(() =>
            {
                // Return 304 
                var tag = Request.Headers.IfNoneMatch.FirstOrDefault();
                if (Request.Headers.IfModifiedSince.HasValue && tag != null && tag.Tag.Length > 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotModified);
                }

                string[] param = fileInfo.Split('.')[0].Split('-');

                // Return 400 参数错误-- id+water-width-height.jpg
                if (param.Length <= 0 || param.Length > 3)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                #region 获取请求参数
                string guid = param[0];
                string water = string.Empty;

                string[] paramGuidWater = guid.Trim('+').Split('+');
                guid = paramGuidWater[0];
                if (paramGuidWater.Length > 1)
                {
                    water = paramGuidWater[1];
                }

                int w = 0, h = 0;
                try
                {
                    if (param.Length > 1)
                    {
                        w = int.Parse(param[1]);
                    }
                    if (param.Length > 2)
                    {
                        h = int.Parse(param[2]);
                    }
                }
                catch (Exception ex)
                {
                    // 取默认值大小 w = 0, h = 0;
                    WriteLog.Instance.Wirtelog("客户端请求图片参数 设置宽高错误", ex);
                }
                #endregion

                return DownImageService.Instance.DownImage(guid, water, w, h);
            });
        }


        /// <summary>
        /// 获取图片外 文件处理  
        /// </summary>
        /// <param name="fileInfo">guidValue.扩展名 文件GUID.扩展名称  </param> 
        [Route("api/down/file/{fileInfo}")]
        public async Task<HttpResponseMessage> GetFile(string fileInfo)
        {
            return await Task.Run(() =>
            { 
                string[] param = fileInfo.Split('.'); 
                //暂不支持 其他文件处理
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            });
        }


    }
}
