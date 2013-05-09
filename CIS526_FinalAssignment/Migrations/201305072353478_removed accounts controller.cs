namespace CIS526_FinalAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedaccountscontroller : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "player_ID", "dbo.Players");
            DropIndex("dbo.Users", new[] { "player_ID" });
            DropTable("dbo.Users");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.Users", "player_ID");
            AddForeignKey("dbo.Users", "player_ID", "dbo.Players", "ID");
        }
    }
}
