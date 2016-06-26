using GL.Common;
using GL.DBOptions.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GL.DBOptions.Services
{
    public class ImageService
    {
        public GL_Images Get(string sId)
        {
            using (var db = new GLDbContext())
            {
                return db.GL_Images.Find(sId);
            }
        }

        public HelperResultMsg InsertAll(List<GL_Images> list)
        {
            HelperResultMsg msg = new HelperResultMsg();
            try
            {
                using (var db = new GLDbContext())
                {
                    foreach (var item in list)
                    {
                        db.GL_Images.Add(item);
                    }
                    db.SaveChanges();

                    msg.success = true;
                    msg.message = "insert success";
                }
            }
            catch (Exception ex)
            {
                HelperNLog.Default.Error(ex.ToString());
                msg.message = "insert error";
            }
            return msg;
        }


        public object GetByAppId(string appId, int pageSize, int pageIndex)
        {
            using (var db = new GLDbContext())
            {
                var result = db.GL_Images
                    .Where(o => o.sAppId == appId && !o.bIsDelete)
                        .Select(o => new
                        {
                            o.sUriDomain,
                            o.sUriPath
                        });
                return result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(); 
            }
        }

    }
}