namespace RyanTube.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UploadViewModels", "file", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UploadViewModels", "file");
        }
    }
}
