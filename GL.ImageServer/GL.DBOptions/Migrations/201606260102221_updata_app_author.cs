namespace GL.DBOptions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updata_app_author : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GL_APP_Authorized", "sAppId", c => c.String(nullable: false, maxLength: 16));
            AlterColumn("dbo.GL_APP_Authorized", "sAppKey", c => c.String(nullable: false, maxLength: 16, fixedLength: true, unicode: false));
            AlterColumn("dbo.GL_APP_Authorized", "sAppSecret", c => c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GL_APP_Authorized", "sAppSecret", c => c.String(nullable: false, maxLength: 16, fixedLength: true, unicode: false));
            AlterColumn("dbo.GL_APP_Authorized", "sAppKey", c => c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false));
            DropColumn("dbo.GL_APP_Authorized", "sAppId");
        }
    }
}
