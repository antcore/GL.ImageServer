namespace GL.DBOptions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_app_appnameadd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GL_APP_Authorized", "sAppName", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GL_APP_Authorized", "sAppName");
        }
    }
}
