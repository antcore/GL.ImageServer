namespace GL.DBOptions.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GLDbContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GL_APP_Authorized",
                c => new
                {
                    sId = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                    sMemberId = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                    sAppKey = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                    sAppSecret = c.String(nullable: false, maxLength: 16, fixedLength: true, unicode: false),
                    bState = c.Boolean(nullable: false),
                    bIsDelete = c.Boolean(nullable: false),
                    dCreateTime = c.DateTime(nullable: false),
                    dUpdateTime = c.DateTime(),
                })
                .PrimaryKey(t => t.sId);

            CreateTable(
                "dbo.GL_CloudSetting",
                c => new
                {
                    sId = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                    sServerName = c.String(nullable: false, maxLength: 30),
                    sServerUriDomain = c.String(nullable: false, maxLength: 50, unicode: false),
                    sSaveDisc = c.String(nullable: false, maxLength: 20, unicode: false),
                    sSavePath = c.String(nullable: false, maxLength: 50, unicode: false),
                    sServerCode = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                    sRemark = c.String(maxLength: 250),
                    bIsDefault = c.Boolean(nullable: false),
                    bState = c.Boolean(nullable: false),
                    bIsDelete = c.Boolean(nullable: false),
                    dCreateTime = c.DateTime(nullable: false),
                    dUpdateTime = c.DateTime(),
                })
                .PrimaryKey(t => t.sId);

            CreateTable(
                "dbo.GL_Directory",
                c => new
                {
                    sId = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                    sAppId = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                    sPId = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                    sDirName = c.String(nullable: false, maxLength: 30),
                    sDirExplain = c.String(maxLength: 250),
                    bIsDefault = c.Boolean(nullable: false),
                    bState = c.Boolean(nullable: false),
                    bIsDelete = c.Boolean(nullable: false),
                    dCreateTime = c.DateTime(nullable: false),
                    dUpdateTime = c.DateTime(),
                })
                .PrimaryKey(t => t.sId);

            CreateTable(
                "dbo.GL_ImageExtendInfo",
                c => new
                {
                    sId = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                    fImgSizeHight = c.Double(),
                    fImgSizeWidth = c.Double(),
                    sProductNum = c.String(maxLength: 128),
                    dPhotoCreatime = c.DateTime(),
                    sLng = c.String(maxLength: 20, unicode: false),
                    sLat = c.String(maxLength: 20, unicode: false),
                })
                .PrimaryKey(t => t.sId);

            CreateTable(
                "dbo.GL_Images",
                c => new
                {
                    sId = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                    sDirId = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                    sUriDomain = c.String(nullable: false, maxLength: 100, unicode: false),
                    sUriPath = c.String(nullable: false, maxLength: 100, unicode: false),
                    iSource = c.Int(nullable: false),
                    sFileName = c.String(maxLength: 100, unicode: false),
                    sFileSiffix = c.String(nullable: false, maxLength: 10, unicode: false),
                    iFileSize = c.Long(nullable: false),
                    sFilePath = c.String(maxLength: 150, unicode: false),
                    bState = c.Boolean(nullable: false),
                    bIsDelete = c.Boolean(nullable: false),
                    dCreateTime = c.DateTime(nullable: false),
                    dUpdateTime = c.DateTime(),
                })
                .PrimaryKey(t => t.sId);

            CreateTable(
                "dbo.GL_Member",
                c => new
                {
                    sId = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                    sUserName = c.String(nullable: false, maxLength: 20),
                    sUserEmail = c.String(maxLength: 30, unicode: false),
                    sUserPhone = c.String(maxLength: 11, unicode: false),
                    sUserPwd = c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false),
                    bState = c.Boolean(nullable: false),
                    bIsDelete = c.Boolean(nullable: false),
                    dCreateTime = c.DateTime(nullable: false),
                    dUpdateTime = c.DateTime(),
                })
                .PrimaryKey(t => t.sId);
        }

        public override void Down()
        {
            DropTable("dbo.GL_Member");
            DropTable("dbo.GL_Images");
            DropTable("dbo.GL_ImageExtendInfo");
            DropTable("dbo.GL_Directory");
            DropTable("dbo.GL_CloudSetting");
            DropTable("dbo.GL_APP_Authorized");
        }
    }
}