namespace CIS526_FinalAssignment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class iconstring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tasks", "image", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tasks", "Image", c => c.Binary());
        }
    }
}
