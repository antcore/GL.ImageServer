using GL.ApiServer.Filter;
using GL.Common;
using GL.ImageOptions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GL.ApiServer.Controllers
{
    public class DownController : ApiController
    {
        /// <summary>
        /// 获取缩略图
        /// guid+water-width-height.jpg
        /// 图片ID+水印名称-宽度-高度 water[t/i/''] -->t/文本水印 i-图片水印 没有则不传值
        /// 如：
        /// http://Ip/api/down/guid+t.jpg --文本水印 原图
        /// http://Ip/api/down/guid+i.jpg --png图片水印 原图
        /// http://Ip/api/down/guid.jpg   --无水印 原图
        /// http://Ip/api/down/guid+t-1000-900.jpg --文本水印 1000*900
        /// http://Ip/api/down/guid+i-1000-900.jpg --png图片水印 1000*900
        /// http://Ip/api/down/guid-1000-900.jpg   --无水印 1000*900
        /// </summary>
        /// <param name="fileInfo">guid+water-width-height.jpg ==》图片ID+水印名称-宽度-高度 water[t/i/''] -->t/文本水印 i-图片水印 没有则不传值</param>
        [Route("api/down/{fileInfo}")]
        [NoSign]
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
                    HelperNLog.Default.Error("[图片服务器]客户端请求图片参数设置宽高错误", ex);
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                #endregion 获取请求参数

                return DownImageService.Instance.DownImage(guid, water, w, h);
            });
        } 
    }
}