namespace CIS526_FinalAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class path3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tasks", "pathID", "dbo.Leaderboards");
            DropIndex("dbo.Tasks", new[] { "pathID" });
            AlterColumn("dbo.Tasks", "pathID", c => c.Int());
            AddForeignKey("dbo.Tasks", "pathID", "dbo.Leaderboards", "ID");
            CreateIndex("dbo.Tasks", "pathID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tasks", new[] { "pathID" });
            DropForeignKey("dbo.Tasks", "pathID", "dbo.Leaderboards");
            AlterColumn("dbo.Tasks", "pathID", c => c.Int(nullable: false));
            CreateIndex("dbo.Tasks", "pathID");
            AddForeignKey("dbo.Tasks", "pathID", "dbo.Leaderboards", "ID", cascadeDelete: true);
        }
    }
}
