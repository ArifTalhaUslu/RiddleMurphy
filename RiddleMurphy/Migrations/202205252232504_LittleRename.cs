namespace RiddleMurphy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LittleRename : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Auctions", "AuctionRiddle_RiddleId", "dbo.Riddles");
            DropIndex("dbo.Auctions", new[] { "AuctionRiddle_RiddleId" });
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        OfferId = c.Int(nullable: false, identity: true),
                        OfferAmount = c.Int(nullable: false),
                        OfferRiddle_RiddleId = c.Int(),
                    })
                .PrimaryKey(t => t.OfferId)
                .ForeignKey("dbo.Riddles", t => t.OfferRiddle_RiddleId)
                .Index(t => t.OfferRiddle_RiddleId);
            
            DropTable("dbo.Auctions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Auctions",
                c => new
                    {
                        AuctionId = c.Int(nullable: false, identity: true),
                        AuctionAmount = c.Int(nullable: false),
                        AuctionRiddle_RiddleId = c.Int(),
                    })
                .PrimaryKey(t => t.AuctionId);
            
            DropForeignKey("dbo.Offers", "OfferRiddle_RiddleId", "dbo.Riddles");
            DropIndex("dbo.Offers", new[] { "OfferRiddle_RiddleId" });
            DropTable("dbo.Offers");
            CreateIndex("dbo.Auctions", "AuctionRiddle_RiddleId");
            AddForeignKey("dbo.Auctions", "AuctionRiddle_RiddleId", "dbo.Riddles", "RiddleId");
        }
    }
}
