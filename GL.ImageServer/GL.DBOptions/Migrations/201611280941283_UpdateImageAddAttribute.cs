namespace GL.DBOptions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateImageAddAttribute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GL_Images", "sVirtual_DirectoryPath", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GL_Images", "sVirtual_DirectoryPath");
        }
    }
}
