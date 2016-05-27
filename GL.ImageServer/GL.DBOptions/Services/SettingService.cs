using GL.DBOptions.Models;
using System.Linq;

namespace GL.DBOptions.Services
{
    public class SettingService
    {
        public GL_CloudSetting GetInit()
        {
            using (var db = new GLDbContext())
            {
                return new GL_CloudSetting
                {
                };
            }
        }

        public GL_CloudSetting GetDefault(string model)
        {
            using (var db = new GLDbContext())
            {
                return db.GL_CloudSetting.FirstOrDefault(o => o.bIsDefault);
            }
        }

        public GL_CloudSetting GetByServerCode(string ServerCode)
        {
            using (var db = new GLDbContext())
            {
                return db.GL_CloudSetting.FirstOrDefault(o => o.sServerCode == ServerCode);
            }
        }
    }
}