using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace GL.ImageOptions
{
    public class DOWNFILE_INIT
    {
        #region 初始化信息
        /// <summary>
        /// 最大内存使用大小
        /// </summary>
        public static readonly long MEMORY_SIZE = 64 * 1024 * 1024;
      
  
        //HttpContext.Current.Server.MapPath("~/App_Data/");
        // ContentType字典
        public static Dictionary<string, string> CONTENT_TYPE = null;
        /// <summary>
        /// 初始化字典
        /// </summary>
        static DOWNFILE_INIT()
        {
            CONTENT_TYPE = new Dictionary<string, string>();
            CONTENT_TYPE.Add("ex", "application/andrew-inset");
            CONTENT_TYPE.Add("hqx", "application/mac-binhex40");
            CONTENT_TYPE.Add("cpt", "application/mac-compactpro");
            CONTENT_TYPE.Add("doc", "application/msword");
            CONTENT_TYPE.Add("docx", "application/msword");
            CONTENT_TYPE.Add("bin", "application/octet-stream");
            CONTENT_TYPE.Add("dms", "application/octet-stream");
            CONTENT_TYPE.Add("lha", "application/octet-stream");
            CONTENT_TYPE.Add("lzh", "application/octet-stream");
            CONTENT_TYPE.Add("exe", "application/octet-stream");
            CONTENT_TYPE.Add("class", "application/octet-stream");
            CONTENT_TYPE.Add("so", "application/octet-stream");
            CONTENT_TYPE.Add("dll", "application/octet-stream");
            CONTENT_TYPE.Add("oda", "application/oda");
            CONTENT_TYPE.Add("pdf", "application/pdf");
            CONTENT_TYPE.Add("ai", "application/postscript");
            CONTENT_TYPE.Add("eps", "application/postscript");
            CONTENT_TYPE.Add("ps", "application/postscript");
            CONTENT_TYPE.Add("smi", "application/smil");
            CONTENT_TYPE.Add("smil", "application/smil");
            CONTENT_TYPE.Add("mif", "application/vnd.mif");
            CONTENT_TYPE.Add("xls", "application/vnd.ms-excel");
            CONTENT_TYPE.Add("xlsx", "application/vnd.ms-excel");
            CONTENT_TYPE.Add("ppt", "application/vnd.ms-powerpoint");
            CONTENT_TYPE.Add("wbxml", "application/vnd.wap.wbxml");
            CONTENT_TYPE.Add("wmlc", "application/vnd.wap.wmlc");
            CONTENT_TYPE.Add("wmlsc", "application/vnd.wap.wmlscriptc");
            CONTENT_TYPE.Add("bcpio", "application/x-bcpio");
            CONTENT_TYPE.Add("vcd", "application/x-cdlink");
            CONTENT_TYPE.Add("pgn", "application/x-chess-pgn");
            CONTENT_TYPE.Add("cpio", "application/x-cpio");
            CONTENT_TYPE.Add("csh", "application/x-csh");
            CONTENT_TYPE.Add("dcr", "application/x-director");
            CONTENT_TYPE.Add("dir", "application/x-director");
            CONTENT_TYPE.Add("dxr", "application/x-director");
            CONTENT_TYPE.Add("dvi", "application/x-dvi");
            CONTENT_TYPE.Add("spl", "application/x-futuresplash");
            CONTENT_TYPE.Add("gtar", "application/x-gtar");
            CONTENT_TYPE.Add("hdf", "application/x-hdf");
            CONTENT_TYPE.Add("js", "application/x-javascript");
            CONTENT_TYPE.Add("skp", "application/x-koan");
            CONTENT_TYPE.Add("skd", "application/x-koan");
            CONTENT_TYPE.Add("skt", "application/x-koan");
            CONTENT_TYPE.Add("skm", "application/x-koan");
            CONTENT_TYPE.Add("latex", "application/x-latex");
            CONTENT_TYPE.Add("nc", "application/x-netcdf");
            CONTENT_TYPE.Add("cdf", "application/x-netcdf");
            CONTENT_TYPE.Add("sh", "application/x-sh");
            CONTENT_TYPE.Add("shar", "application/x-shar");
            CONTENT_TYPE.Add("swf", "application/x-shockwave-flash");
            CONTENT_TYPE.Add("flv", "application/x-shockwave-flash");
            CONTENT_TYPE.Add("sit", "application/x-stuffit");
            CONTENT_TYPE.Add("sv4cpio", "application/x-sv4cpio");
            CONTENT_TYPE.Add("sv4crc", "application/x-sv4crc");
            CONTENT_TYPE.Add("tar", "application/x-tar");
            CONTENT_TYPE.Add("tcl", "application/x-tcl");
            CONTENT_TYPE.Add("tex", "application/x-tex");
            CONTENT_TYPE.Add("texinfo", "application/x-texinfo");
            CONTENT_TYPE.Add("texi", "application/x-texinfo");
            CONTENT_TYPE.Add("t", "application/x-troff");
            CONTENT_TYPE.Add("tr", "application/x-troff");
            CONTENT_TYPE.Add("roff", "application/x-troff");
            CONTENT_TYPE.Add("man", "application/x-troff-man");
            CONTENT_TYPE.Add("me", "application/x-troff-me");
            CONTENT_TYPE.Add("ms", "application/x-troff-ms");
            CONTENT_TYPE.Add("ustar", "application/x-ustar");
            CONTENT_TYPE.Add("src", "application/x-wais-source");
            CONTENT_TYPE.Add("xhtml", "application/xhtml+xml");
            CONTENT_TYPE.Add("xht", "application/xhtml+xml");
            CONTENT_TYPE.Add("zip", "application/zip");
            CONTENT_TYPE.Add("rar", "application/zip");
            CONTENT_TYPE.Add("gz", "application/x-gzip");
            CONTENT_TYPE.Add("bz2", "application/x-bzip2");
            CONTENT_TYPE.Add("au", "audio/basic");
            CONTENT_TYPE.Add("snd", "audio/basic");
            CONTENT_TYPE.Add("mid", "audio/midi");
            CONTENT_TYPE.Add("midi", "audio/midi");
            CONTENT_TYPE.Add("kar", "audio/midi");
            CONTENT_TYPE.Add("mpga", "audio/mpeg");
            CONTENT_TYPE.Add("mp2", "audio/mpeg");
            CONTENT_TYPE.Add("mp3", "audio/mpeg");
            CONTENT_TYPE.Add("aif", "audio/x-aiff");
            CONTENT_TYPE.Add("aiff", "audio/x-aiff");
            CONTENT_TYPE.Add("aifc", "audio/x-aiff");
            CONTENT_TYPE.Add("m3u", "audio/x-mpegurl");
            CONTENT_TYPE.Add("rmm", "audio/x-pn-realaudio");
            CONTENT_TYPE.Add("rmvb", "audio/x-pn-realaudio");
            CONTENT_TYPE.Add("ram", "audio/x-pn-realaudio");
            CONTENT_TYPE.Add("rm", "audio/x-pn-realaudio");
            CONTENT_TYPE.Add("rpm", "audio/x-pn-realaudio-plugin");
            CONTENT_TYPE.Add("ra", "audio/x-realaudio");
            CONTENT_TYPE.Add("wav", "audio/x-wav");
            CONTENT_TYPE.Add("wma", "audio/x-wma");
            CONTENT_TYPE.Add("pdb", "chemical/x-pdb");
            CONTENT_TYPE.Add("xyz", "chemical/x-xyz");
            CONTENT_TYPE.Add("bmp", "image/bmp");
            CONTENT_TYPE.Add("gif", "image/gif");
            CONTENT_TYPE.Add("ief", "image/ief");
            CONTENT_TYPE.Add("jpeg", "image/jpeg");
            CONTENT_TYPE.Add("jpg", "image/jpeg");
            CONTENT_TYPE.Add("jpe", "image/jpeg");
            CONTENT_TYPE.Add("png", "image/png");
            CONTENT_TYPE.Add("tiff", "image/tiff");
            CONTENT_TYPE.Add("tif", "image/tiff");
            CONTENT_TYPE.Add("djvu", "image/vnd.djvu");
            CONTENT_TYPE.Add("djv", "image/vnd.djvu");
            CONTENT_TYPE.Add("wbmp", "image/vnd.wap.wbmp");
            CONTENT_TYPE.Add("ras", "image/x-cmu-raster");
            CONTENT_TYPE.Add("pnm", "image/x-portable-anymap");
            CONTENT_TYPE.Add("pbm", "image/x-portable-bitmap");
            CONTENT_TYPE.Add("pgm", "image/x-portable-graymap");
            CONTENT_TYPE.Add("ppm", "image/x-portable-pixmap");
            CONTENT_TYPE.Add("rgb", "image/x-rgb");
            CONTENT_TYPE.Add("xbm", "image/x-xbitmap");
            CONTENT_TYPE.Add("xpm", "image/x-xpixmap");
            CONTENT_TYPE.Add("xwd", "image/x-xwindowdump");
            CONTENT_TYPE.Add("igs", "model/iges");
            CONTENT_TYPE.Add("iges", "model/iges");
            CONTENT_TYPE.Add("msh", "model/mesh");
            CONTENT_TYPE.Add("mesh", "model/mesh");
            CONTENT_TYPE.Add("silo", "model/mesh");
            CONTENT_TYPE.Add("wrl", "model/vrml");
            CONTENT_TYPE.Add("vrml", "model/vrml");
            CONTENT_TYPE.Add("css", "text/css");
            CONTENT_TYPE.Add("html", "text/html");
            CONTENT_TYPE.Add("htm", "text/html");
            CONTENT_TYPE.Add("asc", "text/plain");
            CONTENT_TYPE.Add("txt", "text/plain");
            CONTENT_TYPE.Add("rtx", "text/richtext");
            CONTENT_TYPE.Add("rtf", "text/rtf");
            CONTENT_TYPE.Add("sgml", "text/sgml");
            CONTENT_TYPE.Add("sgm", "text/sgml");
            CONTENT_TYPE.Add("tsv", "text/tab-separated-values");
            CONTENT_TYPE.Add("wml", "text/vnd.wap.wml");
            CONTENT_TYPE.Add("wmls", "text/vnd.wap.wmlscript");
            CONTENT_TYPE.Add("etx", "text/x-setext");
            CONTENT_TYPE.Add("xsl", "text/xml");
            CONTENT_TYPE.Add("xml", "text/xml");
            CONTENT_TYPE.Add("mpeg", "video/mpeg");
            CONTENT_TYPE.Add("mpg", "video/mpeg");
            CONTENT_TYPE.Add("mpe", "video/mpeg");
            CONTENT_TYPE.Add("qt", "video/quicktime");
            CONTENT_TYPE.Add("mov", "video/quicktime");
            CONTENT_TYPE.Add("mxu", "video/vnd.mpegurl");
            CONTENT_TYPE.Add("avi", "video/x-msvideo");
            CONTENT_TYPE.Add("movie", "video/x-sgi-movie");
            CONTENT_TYPE.Add("wmv", "video/x-ms-wmv");
            CONTENT_TYPE.Add("asf", "video/x-ms-asf");
            CONTENT_TYPE.Add("ice", "x-conference/x-cooltalk");
        }
        #endregion

        #region 基础方法
        /// <summary>
        /// 获得资源类型 Get ContentType
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>ContentType</returns>
        public static string GetContentType(string type)
        {
            // 获取文件对应的ContentType
            try
            {
                string contentType = CONTENT_TYPE[type];
                if (contentType != null)
                    return contentType;
            }
            catch { }
            return "application/octet-stream" + type;
        }
        /// <summary>
        /// 获得资源容 Get ContentDisposition
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>ContentDisposition</returns>
        public static string GetDisposition(string type)
        {
            // 判断使用浏览器打开还是附件模式
            if (GetContentType(type).StartsWith("image"))
            {
                return "inline";
            }
            return "attachment";
        }
        #endregion

    }
}