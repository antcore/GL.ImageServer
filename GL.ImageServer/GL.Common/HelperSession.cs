using System;

namespace GL.Common
{
    public class MemberSessionModel
    {
        public string memberId { get; set; }
        public string memberUserName { get; set; }
    }

    public class HelperSession : IDisposable
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
        private static HelperSession instance;

        public static HelperSession Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HelperSession();
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

        ~HelperSession()
        {
            Dispose(false);
        }

        #endregion 类句柄操作


        public void SetMemberInfo(MemberSessionModel Value)
        {
            Set("GL_IMAGESER_MEMBERINFO", Value);
        }
        public MemberSessionModel GetMemberInfo()
        {
            var obj = Get("GL_IMAGESER_MEMBERINFO");
            return obj == null ? null : obj as MemberSessionModel;
        }



        private void Set(string Key, object Value)
        {
            System.Web.HttpContext.Current.Session[Key] = Value;
        }
        private object Get(string Key)
        {
            return System.Web.HttpContext.Current.Session[Key];
        }

    }
}