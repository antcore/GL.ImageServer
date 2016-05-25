namespace GL.DBOptions
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Models;
    public partial class GLDbContext : DbContext
    {
        public GLDbContext()
            : base("name=GL_ImageServer")
        {
        }

        public virtual DbSet<GL_APP_Authorized> GL_APP_Authorized { get; set; }
        public virtual DbSet<GL_CloudSetting> GL_CloudSetting { get; set; }
        public virtual DbSet<GL_Directory> GL_Directory { get; set; }
        public virtual DbSet<GL_ImageExtendInfo> GL_ImageExtendInfo { get; set; }
        public virtual DbSet<GL_Images> GL_Images { get; set; }
        public virtual DbSet<GL_Member> GL_Member { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GL_APP_Authorized>()
                .Property(e => e.sId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GL_APP_Authorized>()
                .Property(e => e.sMemberId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GL_APP_Authorized>()
                .Property(e => e.sAppKey)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GL_APP_Authorized>()
                .Property(e => e.sAppSecret)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GL_CloudSetting>()
                .Property(e => e.sId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GL_CloudSetting>()
                .Property(e => e.sServerUriDomain)
                .IsUnicode(false);

            modelBuilder.Entity<GL_CloudSetting>()
                .Property(e => e.sSaveDisc)
                .IsUnicode(false);

            modelBuilder.Entity<GL_CloudSetting>()
                .Property(e => e.sSavePath)
                .IsUnicode(false);

            modelBuilder.Entity<GL_CloudSetting>()
                .Property(e => e.sServerCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GL_Directory>()
                .Property(e => e.sId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GL_Directory>()
                .Property(e => e.sAppId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GL_Directory>()
                .Property(e => e.sPId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GL_ImageExtendInfo>()
                .Property(e => e.sId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GL_ImageExtendInfo>()
                .Property(e => e.sLng)
                .IsUnicode(false);

            modelBuilder.Entity<GL_ImageExtendInfo>()
                .Property(e => e.sLat)
                .IsUnicode(false);

            modelBuilder.Entity<GL_Images>()
                .Property(e => e.sId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GL_Images>()
                .Property(e => e.sDirId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GL_Images>()
                .Property(e => e.sUriDomain)
                .IsUnicode(false);

            modelBuilder.Entity<GL_Images>()
                .Property(e => e.sUriPathName)
                .IsUnicode(false);

            modelBuilder.Entity<GL_Images>()
                .Property(e => e.sFileName)
                .IsUnicode(false);

            modelBuilder.Entity<GL_Images>()
                .Property(e => e.sFileSiffix)
                .IsUnicode(false);

            modelBuilder.Entity<GL_Images>()
                .Property(e => e.sFilePath)
                .IsUnicode(false);

            modelBuilder.Entity<GL_Member>()
                .Property(e => e.sId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GL_Member>()
                .Property(e => e.sUserEmail)
                .IsUnicode(false);

            modelBuilder.Entity<GL_Member>()
                .Property(e => e.sUserPhone)
                .IsUnicode(false);

            modelBuilder.Entity<GL_Member>()
                .Property(e => e.sUserPwd)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
