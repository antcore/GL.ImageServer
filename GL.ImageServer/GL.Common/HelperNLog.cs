using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.Common
{
      /// <summary>
      /// 用户记录调试日志
      /// </summary>
      public class HelperNLog
    {
            NLog.Logger logger;

            private HelperNLog(NLog.Logger logger)
            {
                  this.logger = logger;
            }

            public HelperNLog(string name)
                  : this(NLog.LogManager.GetLogger(name))
            {
            }

            public static HelperNLog Default { get; private set; }
            static HelperNLog()
            {
                  Default = new HelperNLog(NLog.LogManager.GetCurrentClassLogger());
            }

            public void Debug(string msg, params object[] args)
            {
                  logger.Debug(msg, args);
            }

         
            public void Info(string msg, params object[] args)
            {
                  logger.Info(msg, args);
            } 
         

            public void Trace(string msg, params object[] args)
            {
                  logger.Trace(msg, args);
            } 

            public void Error(string msg, params object[] args)
            {
                  logger.Error(msg, args);
            } 

            public void Fatal(string msg, params object[] args)
            {
                  logger.Fatal(msg, args);
            }

            
      }
}
