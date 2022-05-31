namespace RiddleMurphy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FollowsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Follows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Followen_UserId = c.Int(),
                        Follower_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Followen_UserId)
                .ForeignKey("dbo.Users", t => t.Follower_UserId)
                .Index(t => t.Followen_UserId)
                .Index(t => t.Follower_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Follows", "Follower_UserId", "dbo.Users");
            DropForeignKey("dbo.Follows", "Followen_UserId", "dbo.Users");
            DropIndex("dbo.Follows", new[] { "Follower_UserId" });
            DropIndex("dbo.Follows", new[] { "Followen_UserId" });
            DropTable("dbo.Follows");
        }
    }
}
