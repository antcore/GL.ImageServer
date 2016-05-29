using System;
using System.IO;
using System.Security.Cryptography;

namespace GL.ImageOptions
{
    public class HashUtils
    {
        /// <summary>
        /// 获取文件的MD5-格式[大写]
        /// </summary>
        /// <param name="fileStream">文件FileStream</param>
        /// <returns>MD5Hash</returns>
        public static string GetMD5Hash(FileStream fileStream)
        {
            try
            {
                MD5 md5Provider = new MD5CryptoServiceProvider();
                byte[] buffer = md5Provider.ComputeHash(fileStream);
                md5Provider.Clear();
                return BitConverter.ToString(buffer).Replace("-", "");
            }
            catch { return null; }
        }

        /// <summary>
        /// 获取字符串的MD5-格式[大写]
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>MD5Hash</returns>
        public static string GetMD5Hash(string str)
        {
            try
            {
                MD5 md5Provider = new MD5CryptoServiceProvider();
                byte[] buffer = md5Provider.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
                md5Provider.Clear();
                return BitConverter.ToString(buffer).Replace("-", "");
            }
            catch { return null; }
        }

        /// <summary>
        /// 计算Byte数组的MD5-格式[大写]
        /// </summary>
        /// <param name="bytes">byte[]</param>
        /// <returns>MD5Hash</returns>
        public static string GetMD5Hash(byte[] bytes)
        {
            try
            {
                MD5 md5Provider = new MD5CryptoServiceProvider();
                byte[] buffer = md5Provider.ComputeHash(bytes);
                md5Provider.Clear();
                return BitConverter.ToString(buffer).Replace("-", "");
            }
            catch { return null; }
        }
    }
}