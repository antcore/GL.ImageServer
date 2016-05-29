namespace GL.DBOptions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_member_attribute : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GL_Member", "sUserName", c => c.String());
            AlterColumn("dbo.GL_Member", "sUserPhone", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GL_Member", "sUserPhone", c => c.String(maxLength: 11, unicode: false));
            AlterColumn("dbo.GL_Member", "sUserName", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
