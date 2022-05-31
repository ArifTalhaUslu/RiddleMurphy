namespace RiddleMurphy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LikesModelAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        LikesId = c.Int(nullable: false, identity: true),
                        LikedRiddle_RiddleId = c.Int(),
                        Liker_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.LikesId)
                .ForeignKey("dbo.Riddles", t => t.LikedRiddle_RiddleId)
                .ForeignKey("dbo.Users", t => t.Liker_UserId)
                .Index(t => t.LikedRiddle_RiddleId)
                .Index(t => t.Liker_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Likes", "Liker_UserId", "dbo.Users");
            DropForeignKey("dbo.Likes", "LikedRiddle_RiddleId", "dbo.Riddles");
            DropIndex("dbo.Likes", new[] { "Liker_UserId" });
            DropIndex("dbo.Likes", new[] { "LikedRiddle_RiddleId" });
            DropTable("dbo.Likes");
        }
    }
}
