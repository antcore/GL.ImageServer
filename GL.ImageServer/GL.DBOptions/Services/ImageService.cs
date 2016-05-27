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
    public class ImageService
    {
          
        public HelperResultMsg Insert(GL_Images model)
        {
            HelperResultMsg msg = new HelperResultMsg();
            try
            {
                using (var db = new GLDbContext())
                {
                    model.sId = HelperAutoGuid.Instance.GetGuidString();
                    model.dCreateTime = DateTime.Now;

                    db.GL_Images.Add(model);
                    db.SaveChanges();

                    msg.success = true;
                    msg.message = "insert success";
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Default.Error(ex.ToString());
                msg.message = "insert error";
            }
            return msg;
        }

        public HelperResultMsg Update(string sid,GL_Images model)
        {
            HelperResultMsg msg = new HelperResultMsg();
            try
            {
                using (var db = new GLDbContext())
                {
                    db.GL_Images
                        .Where(o => o.sId == sid)
                        .Update(o => new GL_Images
                        {
                         
                   

                            dUpdateTime = DateTime.Now
                        }); 
                    db.SaveChanges();

                    msg.success = true;
                    msg.message = "update success";
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Default.Error(ex.ToString());
                msg.message = "update error";
            }
            return msg;
        }


    }


}
