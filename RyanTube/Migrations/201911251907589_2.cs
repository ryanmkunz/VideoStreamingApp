namespace RyanTube.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UploadViewModels", "FileName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UploadViewModels", "FileName");
        }
    }
}
