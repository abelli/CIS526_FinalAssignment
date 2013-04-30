namespace CIS526_FinalAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rankfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PathScores", "rank", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PathScores", "rank");
        }
    }
}
