namespace RiddleMurphy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Creating : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CoinAccounts",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        Balance = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId, unique: true);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50, unicode: false),
                        UserPassword = c.String(nullable: false),
                        UserProfileImgPath = c.String(),
                        UserJoinDate = c.DateTime(nullable: false),
                        UserRole = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .Index(t => t.UserName, unique: true);
            
            CreateTable(
                "dbo.Riddles",
                c => new
                    {
                        RiddleId = c.Int(nullable: false, identity: true),
                        RiddleHeader = c.String(nullable: false),
                        RiddleText = c.String(nullable: false),
                        RiddleAnswer = c.String(nullable: false),
                        RiddlePrize = c.Int(nullable: false),
                        RiddleCost = c.Int(nullable: false),
                        RiddleLike = c.Int(nullable: false),
                        RiddleDislike = c.Int(nullable: false),
                        RiddleState = c.Boolean(nullable: false),
                        Owner_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.RiddleId)
                .ForeignKey("dbo.Users", t => t.Owner_UserId)
                .Index(t => t.Owner_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CoinAccounts", "UserId", "dbo.Users");
            DropForeignKey("dbo.Riddles", "Owner_UserId", "dbo.Users");
            DropIndex("dbo.Riddles", new[] { "Owner_UserId" });
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.CoinAccounts", new[] { "UserId" });
            DropTable("dbo.Riddles");
            DropTable("dbo.Users");
            DropTable("dbo.CoinAccounts");
        }
    }
}
