namespace CIS526_FinalAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class path2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "path_ID", "dbo.Leaderboards");
            DropIndex("dbo.Tasks", new[] { "path_ID" });
            RenameColumn(table: "dbo.Tasks", name: "path_ID", newName: "pathID");
            AddForeignKey("dbo.Tasks", "pathID", "dbo.Leaderboards", "ID", cascadeDelete: true);
            CreateIndex("dbo.Tasks", "pathID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tasks", new[] { "pathID" });
            DropForeignKey("dbo.Tasks", "pathID", "dbo.Leaderboards");
            RenameColumn(table: "dbo.Tasks", name: "pathID", newName: "path_ID");
            CreateIndex("dbo.Tasks", "path_ID");
            AddForeignKey("dbo.Tasks", "path_ID", "dbo.Leaderboards", "ID");
        }
    }
}
