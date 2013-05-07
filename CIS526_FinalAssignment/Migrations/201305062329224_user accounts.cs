namespace CIS526_FinalAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class useraccounts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        username = c.String(nullable: false, maxLength: 100),
                        realName = c.String(nullable: false, maxLength: 100),
                        player_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Players", t => t.player_ID)
                .Index(t => t.player_ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "player_ID" });
            DropForeignKey("dbo.Users", "player_ID", "dbo.Players");
            DropTable("dbo.Users");
        }
    }
}
