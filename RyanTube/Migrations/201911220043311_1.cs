namespace RyanTube.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UploadViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilePath = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UploadViewModels");
        }
    }
}
