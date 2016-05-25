using System;
using System.Runtime.Caching;

namespace GL.Common
{
    
    /// <summary>
    /// 缓存时间设置
    /// </summary>
    public enum CacheTime
    {
        /// <summary>
        /// 30秒
        /// </summary>
        SECONDS_30 = 30,
        /// <summary>
        /// 1分钟
        /// </summary>
        MINUTES_1 = 60,
        /// <summary>
        /// 2分钟
        /// </summary>
        MINUTES_2 = 60 * 2,
        /// <summary>
        /// 5分钟
        /// </summary>
        MINUTES_5 = 60 * 5,
        /// <summary>
        /// 10分钟
        /// </summary>
        MINUTES_10 = 60 * 10
    }

     

    /// <summary>
    /// 缓存帮助
    /// </summary>
    public class HelperMemoryCacher
    {
        private static readonly object lockObj = new object();
        private static HelperMemoryCacher instance;
        static public HelperMemoryCacher Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new HelperMemoryCacher();
                        }
                    }
                }
                return instance;
            }
        }
        #region INIT BASE 
        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Get(key);
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="seconds">缓存时间--秒</param>
        /// <returns></returns>
        public bool SetValue(string key, object value, int seconds)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            return memoryCache.Add(key, value, DateTimeOffset.UtcNow.AddSeconds(seconds));
        }
        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="key">键</param>
        public void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
        }
        #endregion 

    }


}