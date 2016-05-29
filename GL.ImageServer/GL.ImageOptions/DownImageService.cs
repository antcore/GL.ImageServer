using GL.Common;
using GL.DBOptions.Services;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GL.ImageOptions
{
    public class DownImageService : IDisposable
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
        private static DownImageService instance;

        public static DownImageService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DownImageService();
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

        ~DownImageService()
        {
            Dispose(false);
        }

        public DownImageService()
        {
        }

        #endregion 类句柄操作

        public HttpResponseMessage DownImage(string guid, string water, int w, int h)
        {
            //判断参数是否正确
            if (w < 0 || h < 0)
            {
                return GetNotFindImage();
            }
            //查找该文件
            var model = new ImageService().Get(guid);

            if (model == null || !File.Exists(model.sFilePath))
            {
                return GetNotFindImage();
            }

            Bitmap bitmap = null;

            #region 图片处理 非GIF 图片

            if (DOWNFILE_INIT.GetContentType(model.sFileSiffix).StartsWith("image") && DOWNFILE_INIT.GetContentType(model.sFileSiffix).IndexOf("gif") == -1)
            {
                // 打开图片
                bitmap = new Bitmap(model.sFilePath);

                int type = 4;
                // 放大缩小
                if (w >= 0 || h >= 0)
                {
                    w = w < 0 ? bitmap.Width : w;
                    h = h < 0 ? bitmap.Height : h;
                    switch (type)
                    {
                        case 1:
                            bitmap = ImageUtils.Resize(bitmap, w, h);
                            break;

                        case 2:
                            bitmap = ImageUtils.SizeImageWithOldPercent(bitmap, w, h);
                            break;

                        case 3:
                            bitmap = ImageUtils.SizeImage(bitmap, w, h);
                            break;

                        case 4:
                            bitmap = ImageUtils.SizeImageWithOldPercent(bitmap, w, h);
                            break;

                        case 5:
                            bitmap = ImageUtils.SizeImageWithOldPercent(bitmap, w, h, 2);
                            break;

                        case 6:
                            bitmap = ImageUtils.SizeImageWithOldPercent(bitmap, w, h, 3);
                            break;
                    }
                }
                else
                {
                    w = bitmap.Width;
                    h = bitmap.Height;
                    bitmap = ImageUtils.SizeImageWithOldPercent(bitmap, w, h, 3);
                }
            }

            #endregion 图片处理 非GIF 图片

            try
            {
                // Copy To Memory And Close.
                byte[] bytes = null;
                if (bitmap == null)
                {
                    bitmap = new Bitmap(model.sFilePath);
                    //bytes = FileContent(ServerSettingInfo.SaveDisc + model.FilePath);
                }

                #region 加水印

                if (!string.IsNullOrEmpty(water))
                {
                    if (model.sFileSiffix.ToLower().Contains("gif"))
                    {
                    }
                    else
                    {
                        //添加水印-- jpg...
                        bitmap = AppendWaterToImage(water, bitmap);
                    }
                }

                #endregion 加水印

                bytes = ImageUtils.ToBytes(bitmap, model.sFileSiffix);
                // Get Tag
                string eTag = string.Format("\"{0}\"", HashUtils.GetMD5Hash(bytes));

                // 构造返回
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                ContentDispositionHeaderValue disposition = new ContentDispositionHeaderValue(DOWNFILE_INIT.GetDisposition(model.sFileSiffix));
                disposition.FileName = string.Format("{0}.{1}", model.sFileName, model.sFileSiffix);
                disposition.Name = model.sFileName;// model.Name;
                // 这里写入大小为计算后的大小
                disposition.Size = bytes.Length;

                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(DOWNFILE_INIT.GetContentType(model.sFileSiffix));
                result.Content.Headers.ContentDisposition = disposition;
                // Set Cache 这个同Get方法
                result.Content.Headers.Expires = new DateTimeOffset(DateTime.Now).AddHours(1);
                result.Content.Headers.LastModified = new DateTimeOffset(DateTime.Now);
                result.Headers.CacheControl = new CacheControlHeaderValue() { Public = true, MaxAge = TimeSpan.FromHours(1) };
                result.Headers.ETag = new EntityTagHeaderValue(eTag);
                return result;
            }
            catch (Exception ex)
            {
                HelperNLog.Default.Error("[图片服务器]获取图片资源", ex);
            }
            finally
            {
                if (bitmap != null)
                    bitmap.Dispose();
                GC.Collect();
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// 非 gif 图片添加水印
        /// </summary>
        /// <param name="water">水印标志 t 文本水印 i png图片水印</param>
        /// <param name="initImage">需要添加水印图片</param>
        /// <returns></returns>
        private Bitmap AppendWaterToImage(string water, Bitmap initImage)
        {
            //缩略图宽、高计算
            double newWidth = initImage.Width;
            double newHeight = initImage.Height;

            //生成新图
            //新建一个bmp图片
            Bitmap newImage = new Bitmap((int)newWidth, (int)newHeight);
            //新建一个画板
            Graphics newG = Graphics.FromImage(newImage);
            try
            {
                //设置质量
                newG.InterpolationMode = InterpolationMode.HighQualityBicubic;
                newG.SmoothingMode = SmoothingMode.HighQuality;

                //置背景色
                newG.Clear(Color.White);
                //画图
                newG.DrawImage(
                    initImage,
                    new Rectangle(0, 0, newImage.Width, newImage.Height),
                    new Rectangle(0, 0, initImage.Width, initImage.Height),
                    GraphicsUnit.Pixel);

                //文字水印
                if (water.Equals("t"))
                {
                    string watermarkText = "这里是水印！";

                    using (Graphics gWater = Graphics.FromImage(newImage))
                    {
                        Font fontWater = new Font("宋体", 10);
                        Brush brushWater = new SolidBrush(Color.White);
                        gWater.DrawString(watermarkText, fontWater, brushWater, 10, 10);
                        gWater.Dispose();
                    }
                    return newImage;
                }
                //透明图片水印
                if (water.Equals("i"))
                {
                    string waterImgPath = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\InitImages\\1-water.png";

                    if (string.IsNullOrEmpty(waterImgPath) || !File.Exists(waterImgPath))
                    {
                        return initImage;
                    }

                    using (Image wrImage = Image.FromFile(waterImgPath))
                    {
                        //水印绘制条件：原始图片宽高均大于或等于水印图片
                        if (newImage.Width >= wrImage.Width && newImage.Height >= wrImage.Height)
                        {
                            Graphics gWater = Graphics.FromImage(newImage);
                            //透明属性
                            ImageAttributes imgAttributes = new ImageAttributes();
                            ColorMap colorMap = new ColorMap();
                            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                            ColorMap[] remapTable = { colorMap };
                            imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
                            float[][] colorMatrixElements = {
                                   new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5
                                   new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                };
                            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                            imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                            gWater.DrawImage(wrImage, new Rectangle(newImage.Width - wrImage.Width, newImage.Height - wrImage.Height, wrImage.Width, wrImage.Height), 0, 0, wrImage.Width, wrImage.Height, GraphicsUnit.Pixel, imgAttributes);
                            gWater.Dispose();
                        }
                        wrImage.Dispose();
                    }
                }
                return newImage;
            }
            catch (Exception ex)
            {
                HelperNLog.Default.Error("[图片服务器]绘制水印", ex);
                return initImage;
            }
            finally
            {
                newG.Dispose();
                //newImage.Dispose();
                //initImage.Dispose();
            }
        }

        public HttpResponseMessage GetNotFindImage()
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NotFound);
            try
            {
                string FileSiffix = "jpg";
                string FileName = "notfind";

                string notfind = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\InitImages\\1-notfind.jpg";
                // 打开图片
                Bitmap bitmap = new Bitmap(notfind);

                byte[] bytes = ImageUtils.ToBytes(bitmap, FileSiffix);

                // Get Tag
                string eTag = string.Format("\"{0}\"", HashUtils.GetMD5Hash(bytes));

                ContentDispositionHeaderValue disposition = new ContentDispositionHeaderValue(DOWNFILE_INIT.GetDisposition(FileSiffix));
                disposition.FileName = string.Format("{0}.{1}", FileName, FileSiffix);
                disposition.Name = FileName;// model.Name;
                // 这里写入大小为计算后的大小
                disposition.Size = bytes.Length;

                result = new HttpResponseMessage(HttpStatusCode.OK);

                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(DOWNFILE_INIT.GetContentType(FileSiffix));
                result.Content.Headers.ContentDisposition = disposition;
                // Set Cache 这个同Get方法
                result.Content.Headers.Expires = new DateTimeOffset(DateTime.Now).AddHours(1);
                result.Content.Headers.LastModified = new DateTimeOffset(DateTime.Now);
                result.Headers.CacheControl = new CacheControlHeaderValue() { Public = true, MaxAge = TimeSpan.FromHours(1) };
                result.Headers.ETag = new EntityTagHeaderValue(eTag);
            }
            catch (Exception ex)
            {
                HelperNLog.Default.Error("[图片服务器]没找到图片异常", ex);
            }
            return result;
        }

        /// <summary>
        /// Files the content.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public byte[] FileContent(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            try
            {
                byte[] buffur = new byte[fs.Length];
                fs.Read(buffur, 0, (int)fs.Length);
                return buffur;
            }
            catch (Exception ex)
            {
                HelperNLog.Default.Error("[图片服务器]FileContent", ex);
                return null;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();//关闭资源
                }
            }
        }
    }
}