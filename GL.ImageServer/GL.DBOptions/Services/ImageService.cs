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

     

    }


}
