using EntityFramework.Extensions;
using GL.Common;
using GL.DBOptions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GL.DBOptions.Services
{

    public class AppService
    {

        public object GetAll(string memberId)
        {
            using (var db = new GLDbContext())
            {
                return db.GL_APP_Authorized
                        .Where(o => o.sMemberId == memberId && !o.bIsDelete)
                        .Select(o => new
                        {
                            o.sId,
                            o.sAppName,
                            o.sAppKey,
                            o.sAppSecret,
                            o.bState,
                            o.dCreateTime,
                            o.dUpdateTime
                        })
                        .ToList();
            }
        }  
        public string GetSecretByKey(string key)
        {
            using (var db = new GLDbContext())
            {
                var result = db.GL_APP_Authorized.Where(o => o.sAppKey == key && !o.bIsDelete).Select(o => new { o.sAppSecret }).FirstOrDefault(); 
                if (null == result || string.IsNullOrEmpty(result.sAppSecret))
                {
                    return string.Empty;
                }
                return result.sAppSecret;
            }
        }


        public HelperResultMsg Insert(GL_APP_Authorized model)
        {
            HelperResultMsg msg = new HelperResultMsg();
            try
            {
                using (var db = new GLDbContext())
                {
                    model.sId = HelperAutoGuid.Instance.GetGuidString();
                    model.dCreateTime = DateTime.Now;

                    string privateString = model.sId + model.sAppName + model.sMemberId;
                    //app Secret
                    model.sAppSecret = HelperMD5.MD5(privateString);
                    //app key 
                    model.sAppKey = HelperMD5.MD532(privateString + model.dCreateTime.ToString("yyyyMMddHHmmssfff"));

                    db.GL_APP_Authorized.Add(model);
                    db.SaveChanges();

                    msg.success = true;
                    msg.message = "添加应用成功";
                }
            }
            catch (Exception ex)
            {
                HelperNLog.Default.Error(ex.ToString());
                msg.message = "添加应用失败";
            }
            return msg;
        }




    }


}
