namespace CIS526_FinalAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Leaderboards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        pathName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PathScores",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        playerID = c.Int(),
                        leaderboardID = c.Int(),
                        score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Players", t => t.playerID)
                .ForeignKey("dbo.Leaderboards", t => t.leaderboardID)
                .Index(t => t.playerID)
                .Index(t => t.leaderboardID);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        password = c.String(),
                        totalScore = c.Int(nullable: false),
                        isFrozen = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PlayerTasks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        playerID = c.Int(),
                        taskID = c.Int(),
                        pointsEarned = c.Int(nullable: false),
                        completionTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Players", t => t.playerID)
                .ForeignKey("dbo.Tasks", t => t.taskID)
                .Index(t => t.playerID)
                .Index(t => t.taskID);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        pathName = c.String(),
                        startDate = c.DateTime(nullable: false),
                        endDate = c.DateTime(nullable: false),
                        Image = c.Binary(),
                        taskName = c.String(),
                        description = c.String(),
                        pointTotal = c.Int(nullable: false),
                        isMilestone = c.Boolean(nullable: false),
                        milestoneBonus = c.Int(nullable: false),
                        solution = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.PlayerTasks", new[] { "taskID" });
            DropIndex("dbo.PlayerTasks", new[] { "playerID" });
            DropIndex("dbo.PathScores", new[] { "leaderboardID" });
            DropIndex("dbo.PathScores", new[] { "playerID" });
            DropForeignKey("dbo.PlayerTasks", "taskID", "dbo.Tasks");
            DropForeignKey("dbo.PlayerTasks", "playerID", "dbo.Players");
            DropForeignKey("dbo.PathScores", "leaderboardID", "dbo.Leaderboards");
            DropForeignKey("dbo.PathScores", "playerID", "dbo.Players");
            DropTable("dbo.Tasks");
            DropTable("dbo.PlayerTasks");
            DropTable("dbo.Players");
            DropTable("dbo.PathScores");
            DropTable("dbo.Leaderboards");
        }
    }
}
