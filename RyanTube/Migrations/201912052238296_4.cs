﻿namespace RyanTube.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UploadViewModels", "FilePath", c => c.String());
            AlterColumn("dbo.UploadViewModels", "FileName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UploadViewModels", "FileName", c => c.String(nullable: false));
            AlterColumn("dbo.UploadViewModels", "FilePath", c => c.String(nullable: false));
        }
    }
}
