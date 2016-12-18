namespace ClubManager.Migrations.ClubMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClubEvent",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        Venue = c.String(),
                        Location = c.String(),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        ClubId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Club", t => t.ClubId, cascadeDelete: true)
                .Index(t => t.ClubId);
            
            CreateTable(
                "dbo.Club",
                c => new
                    {
                        ClubId = c.Int(nullable: false, identity: true),
                        ClubName = c.String(),
                        CreationDate = c.DateTime(nullable: false, storeType: "date"),
                        AdminId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClubId);
            
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        MemberId = c.Int(nullable: false, identity: true),
                        ClubId = c.Int(nullable: false),
                        StudentId = c.String(nullable: false, maxLength: 128),
                        Approved = c.Boolean(nullable: false),
                        ClubEvent_EventId = c.Int(),
                    })
                .PrimaryKey(t => new { t.MemberId, t.ClubId, t.StudentId })
                .ForeignKey("dbo.Club", t => t.ClubId, cascadeDelete: true)
                .ForeignKey("dbo.Student", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.ClubEvent", t => t.ClubEvent_EventId)
                .Index(t => t.ClubId)
                .Index(t => t.StudentId)
                .Index(t => t.ClubEvent_EventId);
            
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        StudentId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Member", "ClubEvent_EventId", "dbo.ClubEvent");
            DropForeignKey("dbo.ClubEvent", "ClubId", "dbo.Club");
            DropForeignKey("dbo.Member", "StudentId", "dbo.Student");
            DropForeignKey("dbo.Member", "ClubId", "dbo.Club");
            DropIndex("dbo.Member", new[] { "ClubEvent_EventId" });
            DropIndex("dbo.Member", new[] { "StudentId" });
            DropIndex("dbo.Member", new[] { "ClubId" });
            DropIndex("dbo.ClubEvent", new[] { "ClubId" });
            DropTable("dbo.Student");
            DropTable("dbo.Member");
            DropTable("dbo.Club");
            DropTable("dbo.ClubEvent");
        }
    }
}
