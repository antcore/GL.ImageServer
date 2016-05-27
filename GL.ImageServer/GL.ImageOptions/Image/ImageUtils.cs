using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.ImageOptions
{
    /// <summary>
    /// 图片处理类型
    /// </summary>
   public class ImageUtils
    {
        /// <summary>
        /// 图片类型对应的ImageFormat
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>ImageFormat</returns>
        public static ImageFormat GetFormat(string type)
        {
            Dictionary<string, ImageFormat> types = new Dictionary<string, ImageFormat>();
            types.Add("bmp", ImageFormat.Bmp);
            types.Add("gif", ImageFormat.Gif);
            types.Add("ief", ImageFormat.Jpeg);
            types.Add("jpeg", ImageFormat.Jpeg);
            types.Add("jpg", ImageFormat.Jpeg);
            types.Add("jpe", ImageFormat.Jpeg);
            types.Add("png", ImageFormat.Png);
            types.Add("tiff", ImageFormat.Tiff);
            types.Add("tif", ImageFormat.Tiff);
            types.Add("djvu", ImageFormat.Bmp);
            types.Add("djv", ImageFormat.Bmp);
            types.Add("wbmp", ImageFormat.Bmp);
            types.Add("ras", ImageFormat.Bmp);
            types.Add("pnm", ImageFormat.Bmp);
            types.Add("pbm", ImageFormat.Bmp);
            types.Add("pgm", ImageFormat.Bmp);
            types.Add("ppm", ImageFormat.Bmp);
            types.Add("rgb", ImageFormat.Bmp);
            types.Add("xbm", ImageFormat.Bmp);
            types.Add("xpm", ImageFormat.Bmp);
            types.Add("xwd", ImageFormat.Bmp);
            types.Add("ico", ImageFormat.Icon);
            types.Add("wmf", ImageFormat.Emf);
            types.Add("exif", ImageFormat.Exif);
            types.Add("emf", ImageFormat.Emf);

            try
            {
                ImageFormat format = types[type];
                if (format != null)
                    return format;
            }
            catch { }
            return ImageFormat.Bmp;
        }


        /// <summary>
        /// 图片转换为字节
        /// </summary>
        /// <param name="bitmap">图片</param>
        /// <param name="type">类型</param>
        /// <returns>字节码</returns>
        public static byte[] ToBytes(Bitmap bitmap, string type)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, GetFormat(type));
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }

        /// <summary>  
        ///  调整图片宽度高度   
        /// </summary>  
        /// <param name="bmp">原始Bitmap </param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>  
        /// <returns>处理Bitmap</returns>  
        public static Bitmap Resize(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量   
                g.InterpolationMode = InterpolationMode.Default;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 切片Bitmap：指定图片分割为 Row 行，Col 列，取第 N 个图片
        /// </summary>
        /// <param name="b">等待分割的图片</param>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        /// <param name="n">取第N个图片</param>
        /// <returns>切片后的Bitmap</returns>
        public static Bitmap Cut(Bitmap b, int row, int col, int n)
        {
            int w = b.Width / col;
            int h = b.Height / row;

            n = n > (col * row) ? col * row : n;

            int x = (n - 1) % col;
            int y = (n - 1) / col;

            x = w * x;
            y = h * y;

            try
            {
                Bitmap bmpOut = new Bitmap(w, h, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, w, h), new Rectangle(x, y, w, h), GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }

        #region 新增加的图片处理 2014-12-22

        /// <summary> 
        /// 按照比例缩小图片 
        /// </summary> 
        /// <param name="srcImage">要缩小的图片</param> 
        /// <param name="percent">缩小比例</param> 
        /// <returns>缩小后的结果</returns> 
        public static Bitmap PercentImage(Bitmap srcImage, double percent)
        {
            // 缩小后的高度 
            int newH = int.Parse(Math.Round(srcImage.Height * percent).ToString());
            // 缩小后的宽度 
            int newW = int.Parse(Math.Round(srcImage.Width * percent).ToString());
            try
            {
                // 要保存到的图片 
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量 
                g.InterpolationMode = InterpolationMode.Default;
                g.DrawImage(srcImage, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary> 
        /// 按照指定大小缩放图片 
        /// </summary> 
        /// <param name="srcImage"></param> 
        /// <param name="iWidth">缩放的宽度</param> 
        /// <param name="iHeight">缩放的高度</param> 
        /// <returns></returns> 
        public static Bitmap SizeImage(Bitmap srcImage, int iWidth, int iHeight)
        {
            try
            {
                // 要保存到的图片 
                Bitmap b = new Bitmap(iWidth, iHeight);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量 
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(srcImage, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary> 
        /// 按照指定大小缩放图片，但是为了保证图片宽高比自动截取 
        /// </summary> 
        /// <param name="srcImage"></param> 
        /// <param name="iWidth"></param> 
        /// <param name="iHeight"></param> 
        /// <returns></returns> 
        public static Bitmap SizeImageWithOldPercent(Bitmap srcImage, int iWidth, int iHeight, int iType = 1)
        {
            try
            {
                // 要截取图片的宽度（临时图片） 
                int newW = srcImage.Width;
                // 要截取图片的高度（临时图片） 
                int newH = srcImage.Height;
                // 截取开始横坐标（临时图片） 
                int newX = 0;
                // 截取开始纵坐标（临时图片） 
                int newY = 0;
                // 截取比例（临时图片） 

                if (iHeight == 0)
                {
                    iHeight = iWidth * newH / newW;
                }

                if (iWidth == 0)
                {
                    iWidth = iHeight * newW / newH;
                }
                double whPercent = 1;
                whPercent = ((double)iWidth / (double)iHeight) * ((double)srcImage.Height / (double)srcImage.Width);
                if (whPercent > 1)
                {
                    // 当前图片宽度对于要截取比例过大时 
                    newH = int.Parse(Math.Round(srcImage.Height / whPercent).ToString());
                }
                else if (whPercent < 1)
                {
                    // 当前图片高度对于要截取比例过大时 
                    newW = int.Parse(Math.Round(srcImage.Width * whPercent).ToString());
                }
                switch (iType)
                {

                    case 1: //全部居中剪切
                        if (newW != srcImage.Width)
                        {
                            // 宽度有变化时，调整开始截取的横坐标 
                            newX = Math.Abs(int.Parse(Math.Round(((double)srcImage.Width - newW) / 2).ToString()));
                        }
                        else if (newH != srcImage.Height)
                        {
                            // 高度有变化时，调整开始截取的纵坐标 
                            newY = Math.Abs(int.Parse(Math.Round(((double)srcImage.Height - (double)newH) / 2).ToString()));
                        }
                        break;
                    case 2://水平居中剪切
                        if (newW != srcImage.Width)
                        {
                            // 宽度有变化时，调整开始截取的横坐标 
                            newX = Math.Abs(int.Parse(Math.Round(((double)srcImage.Width - newW) / 2).ToString()));
                        }
                        else if (newH == srcImage.Height)
                        {
                            // 高度有变化时，调整开始截取的纵坐标 
                            newY = Math.Abs(int.Parse(Math.Round(((double)srcImage.Height - (double)newH) / 2).ToString()));
                        }
                        break;
                    default://左上角剪切
                        newX = 0;
                        newY = 0;
                        break;

                }

                // 取得符合比例的临时文件 
                Bitmap cutedImage = CutImage(srcImage, newX, newY, newW, newH);
                // 保存到的文件 
                Bitmap b = new Bitmap(iWidth, iHeight);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量 
                g.InterpolationMode = InterpolationMode.Default;
                g.DrawImage(cutedImage, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(0, 0, cutedImage.Width, cutedImage.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch (Exception)
            {
                return null;
            }
        }


        ///// <summary> 
        ///// 按照指定大小缩放图片，但是为了保证图片宽高比自动截取 
        ///// </summary> 
        ///// <param name="srcImage"></param> 
        ///// <param name="iWidth"></param> 
        ///// <param name="iHeight"></param> 
        ///// <returns></returns> 
        //public static Bitmap SizeImageWithOldPercent(Bitmap srcImage, int iWidth, int iHeight)
        //{
        //    try
        //    {
        //        // 要截取图片的宽度（临时图片） 
        //        int newW = srcImage.Width;
        //        // 要截取图片的高度（临时图片） 
        //        int newH = srcImage.Height;
        //        // 截取开始横坐标（临时图片） 
        //        int newX = 0;
        //        // 截取开始纵坐标（临时图片） 
        //        int newY = 0;
        //        // 截取比例（临时图片） 
        //        double whPercent = 1;
        //        whPercent = ((double)iWidth / (double)iHeight) * ((double)srcImage.Height / (double)srcImage.Width);
        //        if (whPercent > 1)
        //        {
        //            // 当前图片宽度对于要截取比例过大时 
        //            newW = int.Parse(Math.Round(srcImage.Width / whPercent).ToString());
        //        }
        //        else if (whPercent < 1)
        //        {
        //            // 当前图片高度对于要截取比例过大时 
        //            newH = int.Parse(Math.Round(srcImage.Height * whPercent).ToString());
        //        }
        //        if (newW != srcImage.Width)
        //        {
        //            // 宽度有变化时，调整开始截取的横坐标 
        //            newX = Math.Abs(int.Parse(Math.Round(((double)srcImage.Width - newW) / 2).ToString()));
        //        }
        //        else if (newH == srcImage.Height)
        //        {
        //            // 高度有变化时，调整开始截取的纵坐标 
        //            newY = Math.Abs(int.Parse(Math.Round(((double)srcImage.Height - (double)newH) / 2).ToString()));
        //        }
        //        // 取得符合比例的临时文件 
        //        Bitmap cutedImage = CutImage(srcImage, newX, newY, newW, newH);
        //        // 保存到的文件 
        //        Bitmap b = new Bitmap(iWidth, iHeight);
        //        Graphics g = Graphics.FromImage(b);
        //        // 插值算法的质量 
        //        g.InterpolationMode = InterpolationMode.Default;
        //        g.DrawImage(cutedImage, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(0, 0, cutedImage.Width, cutedImage.Height), GraphicsUnit.Pixel);
        //        g.Dispose();
        //        return b;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        /// <summary> 
        /// jpeg图片压缩 
        /// </summary> 
        /// <param name="sFile">图片地址</param> 
        /// <param name="outPath">输出地址</param> 
        /// <param name="flag">设置压缩的比例1-100</param> 
        /// <returns></returns> 
        public static bool GetPicThumbnail(string sFile, string outPath, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            //以下代码为保存图片时，设置压缩质量 
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100 
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    iSource.Save(outPath, jpegICIinfo, ep);//dFile是压缩后的新路径 
                }
                else
                {
                    iSource.Save(outPath, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                iSource.Dispose();
            }
        }

        /// <summary> 
        /// 剪裁 -- 用GDI+ 
        /// </summary> 
        /// <param name="b">原始Bitmap</param> 
        /// <param name="StartX">开始坐标X</param> 
        /// <param name="StartY">开始坐标Y</param> 
        /// <param name="iWidth">宽度</param> 
        /// <param name="iHeight">高度</param> 
        /// <returns>剪裁后的Bitmap</returns> 
        public static Bitmap CutImage(Bitmap b, int StartX, int StartY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }
            int w = b.Width;
            int h = b.Height;
            if (StartX >= w || StartY >= h)
            {
                // 开始截取坐标过大时，结束处理 
                return null;
            }
            if (StartX + iWidth > w)
            {
                // 宽度过大时只截取到最大大小 
                iWidth = w - StartX;
            }
            if (StartY + iHeight > h)
            {
                // 高度过大时只截取到最大大小 
                iHeight = h - StartY;
            }
            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }


        #endregion
    }
}
