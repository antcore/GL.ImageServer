using System;
using System.IO;
using System.Text;

namespace GL.ImageOptions
{
    public class ReadFile : IDisposable
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
        private static ReadFile instance;

        public static ReadFile Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ReadFile();
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

        ~ReadFile()
        {
            Dispose(false);
        }

        #endregion 类句柄操作

        public string GetFileConten(string filePath)
        {
            string result = string.Empty;
            if (File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                result = sr.ReadToEnd();
                sr.Close();
                fs.Close();
            }
            return result;
        }
    }
}