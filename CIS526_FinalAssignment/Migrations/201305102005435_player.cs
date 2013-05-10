namespace CIS526_FinalAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class player : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "hasChanged", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Players", "hasChanged");
        }
    }
}
