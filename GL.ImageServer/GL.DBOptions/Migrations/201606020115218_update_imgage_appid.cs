namespace GL.DBOptions.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class update_imgage_appid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GL_Images", "sAppId", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.GL_Images", "sDirId", c => c.String(maxLength: 32, fixedLength: true, unicode: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.GL_Images", "sDirId", c => c.String(nullable: false, maxLength: 32, fixedLength: true, unicode: false));
            DropColumn("dbo.GL_Images", "sAppId");
        }
    }
}