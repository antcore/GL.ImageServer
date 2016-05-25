using System;

namespace GL.Common
{
    /* * *
     * 使用方法
     *
     *  string pkid = HelperAutoGuid.Instance.GetGuidString();
     *
     * * */

    public class HelperAutoGuid : IDisposable
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
        private static HelperAutoGuid instance;

        public static HelperAutoGuid Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HelperAutoGuid();
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

        ~HelperAutoGuid()
        {
            Dispose(false);
        }

        #endregion 类句柄操作

        /// <summary>
        /// 获取一个没有横线的guid字符串 32位
        /// </summary>
        /// <returns></returns>
        public string GetGuidString()
        {
            return NewCombId().ToString("N");
        }

        /// <summary>
        /// 获得有顺序的GUID，用Guid前10位加时间参数生成，时间加在最前面6个字节
        /// 需要注意的是，在SQL SERVER数据库中，使用GUID字段类型保存的话，SQL SERVER对GUID类型字段排序算法是以最后6字节为主要依据，
        /// 这与Oracle不同，为了保证排序规则与Oracle一致，在SQL SERVER中要使用Binary(16)数据类型来保存。
        /// </summary>
        /// <returns></returns>
        private Guid NewCombId()
        {
            byte[] guidArray = System.Guid.NewGuid().ToByteArray();

            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            // Get the days and milliseconds which will be used to build the byte string
            TimeSpan days = new TimeSpan(now.Date.Ticks - baseDate.Ticks);
            TimeSpan msecs = new TimeSpan(now.Ticks - (now.Date.Ticks));

            // Convert to a byte array
            // SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            for (int i = 15; i >= 6; i--)
            {
                guidArray[i] = guidArray[i - 6];
            }

            Array.Copy(daysArray, daysArray.Length - 2, guidArray, 0, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, 2, 4);

            return new System.Guid(guidArray);
        }

        //SqlServer实现的代码是：

        //create function [dbo].[GetCombId](@newid uniqueidentifier)
        //returns binary(16)
        //as
        //begin
        //declare @aguid binary(16)

        //set @aguid = cast(
        //cast(getdate() as binary(6))
        //+cast(@newid as binary(10))

        //               as binary(16))
        //return  @aguid
        //end

        //GO
    }
}