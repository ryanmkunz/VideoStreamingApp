namespace RyanTube.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UploadViewModels", "FilePath");
            DropColumn("dbo.UploadViewModels", "FileName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UploadViewModels", "FileName", c => c.String());
            AddColumn("dbo.UploadViewModels", "FilePath", c => c.String());
        }
    }
}
