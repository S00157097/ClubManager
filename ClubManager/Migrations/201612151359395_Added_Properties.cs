namespace ClubManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Properties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "StudentId", c => c.String());
            AddColumn("dbo.AspNetUsers", "DateJoined", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DateJoined");
            DropColumn("dbo.AspNetUsers", "StudentId");
        }
    }
}
