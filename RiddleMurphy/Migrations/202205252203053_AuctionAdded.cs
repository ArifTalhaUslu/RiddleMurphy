namespace RiddleMurphy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuctionAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auctions",
                c => new
                    {
                        AuctionId = c.Int(nullable: false, identity: true),
                        AuctionAmount = c.Int(nullable: false),
                        AuctionRiddle_RiddleId = c.Int(),
                    })
                .PrimaryKey(t => t.AuctionId)
                .ForeignKey("dbo.Riddles", t => t.AuctionRiddle_RiddleId)
                .Index(t => t.AuctionRiddle_RiddleId);
            
            AddColumn("dbo.Riddles", "isTodaysRiddle", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Auctions", "AuctionRiddle_RiddleId", "dbo.Riddles");
            DropIndex("dbo.Auctions", new[] { "AuctionRiddle_RiddleId" });
            DropColumn("dbo.Riddles", "isTodaysRiddle");
            DropTable("dbo.Auctions");
        }
    }
}
