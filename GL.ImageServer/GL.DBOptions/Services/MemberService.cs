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
    public class MemberService
    {

        public HelperResultMsg Login(string sUserName, string sUserPwd)
        {
            HelperResultMsg msg = new HelperResultMsg();
            try
            {
                using (var db = new GLDbContext())
                {
                    var model = db.GL_Member.FirstOrDefault(o => o.sUserName == sUserName);
                    if (null != model)
                    {
                        if (model.sUserPwd.Equals(HelperMD5.MD532(sUserPwd)))
                        {
                            msg.success = true;
                            msg.message = "login success";

                            msg.resultInfo = new MemberSessionModel
                            {
                                memberId = model.sId,
                                memberUserName = model.sUserName
                            };
                        }
                        else
                        {
                            msg.message = "error user password";
                        }
                    }
                    else
                    {
                        msg.message = "error user name";
                    }
                }
            }
            catch (Exception ex)
            {
                HelperNLog.Default.Error(ex.ToString());
                msg.message = "error! once again";
            }
            return msg;
        }

        public HelperResultMsg Insert(GL_Member model)
        {
            HelperResultMsg msg = new HelperResultMsg();
            try
            {
                using (var db = new GLDbContext())
                {
                    //add user
                    model.sId = HelperAutoGuid.Instance.GetGuidString();
                    model.dCreateTime = DateTime.Now;
                    model.sUserPwd = HelperMD5.MD532(model.sUserPwd);
                    db.GL_Member.Add(model);
                    //add application
                    var app = new GL_APP_Authorized
                    {
                        sId = HelperAutoGuid.Instance.GetGuidString(),
                        sMemberId = model.sId,
                        sAppKey = "",
                        sAppSecret = "",
                        dCreateTime = DateTime.Now
                    };
                    db.GL_APP_Authorized.Add(app);
                    //add default directory
                    var directory = new GL_Directory
                    {
                        sId = HelperAutoGuid.Instance.GetGuidString(),
                        sPId = "",
                        sAppId = app.sId,
                        sDirName = "all",
                        sDirExplain = "",
                        bIsDefault = true,
                        dCreateTime = DateTime.Now
                    };
                    db.GL_Directory.Add(directory);
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

        public HelperResultMsg Update(string sid, GL_Member model)
        {
            HelperResultMsg msg = new HelperResultMsg();
            try
            {
                using (var db = new GLDbContext())
                {
                    db.GL_Member
                        .Where(o => o.sId == sid)
                        .Update(o => new GL_Member
                        {
                            sUserEmail = model.sUserEmail,
                            //sUserName = model.sUserName,
                            //sUserPhone = model.sUserPhone,  
                            dUpdateTime = DateTime.Now

                        });
                    db.SaveChanges();

                    msg.success = true;
                    msg.message = "update success";
                }
            }
            catch (Exception ex)
            {
                HelperNLog.Default.Error(ex.ToString());
                msg.message = "update error";
            }
            return msg;
        }

        public HelperResultMsg UpdatePwdById(string sid, string oldPwd, string newPwd)
        {
            HelperResultMsg msg = new HelperResultMsg();
            try
            {
                using (var db = new GLDbContext())
                { 
                    db.GL_Member
                        .Where(o => o.sId == sid)
                        .Update(o => new GL_Member
                        {
                           
                            //sUserName = model.sUserName,
                            //sUserPhone = model.sUserPhone,  
                            dUpdateTime = DateTime.Now

                        });
                    db.SaveChanges();

                    msg.success = true;
                    msg.message = "update success";
                }
            }
            catch (Exception ex)
            {
                HelperNLog.Default.Error(ex.ToString());
                msg.message = "update error";
            }
            return msg;
        }

        public HelperResultMsg UpdatePwdByEmail(string sid, string newPwd)
        {
            HelperResultMsg msg = new HelperResultMsg();
            try
            {
                using (var db = new GLDbContext())
                {
                    db.GL_Member
                        .Where(o => o.sId == sid)
                        .Update(o => new GL_Member
                        { 
                            //sUserName = model.sUserName,
                            //sUserPhone = model.sUserPhone,  
                            dUpdateTime = DateTime.Now

                        });
                    db.SaveChanges();

                    msg.success = true;
                    msg.message = "update success";
                }
            }
            catch (Exception ex)
            {
                HelperNLog.Default.Error(ex.ToString());
                msg.message = "update error";
            }
            return msg;
        }



    }


}
