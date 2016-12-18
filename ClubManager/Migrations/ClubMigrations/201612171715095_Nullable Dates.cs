namespace ClubManager.Migrations.ClubMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableDates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ClubEvent", "StartDateTime", c => c.DateTime());
            AlterColumn("dbo.ClubEvent", "EndDateTime", c => c.DateTime());
            AlterColumn("dbo.Club", "CreationDate", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Club", "CreationDate", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.ClubEvent", "EndDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ClubEvent", "StartDateTime", c => c.DateTime(nullable: false));
        }
    }
}
