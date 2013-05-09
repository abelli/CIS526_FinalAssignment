namespace CIS526_FinalAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class path : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "path_ID", c => c.Int());
            AddForeignKey("dbo.Tasks", "path_ID", "dbo.Leaderboards", "ID");
            CreateIndex("dbo.Tasks", "path_ID");
            DropColumn("dbo.Tasks", "pathName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tasks", "pathName", c => c.String());
            DropIndex("dbo.Tasks", new[] { "path_ID" });
            DropForeignKey("dbo.Tasks", "path_ID", "dbo.Leaderboards");
            DropColumn("dbo.Tasks", "path_ID");
        }
    }
}
