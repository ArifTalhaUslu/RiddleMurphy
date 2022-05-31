namespace RiddleMurphy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProcessModelCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountProcesses",
                c => new
                    {
                        ProcessId = c.Int(nullable: false, identity: true),
                        ProcessType = c.String(maxLength: 50, unicode: false),
                        IsProcesPositive = c.Boolean(nullable: false),
                        ProcessAmount = c.Long(nullable: false),
                        ProcessTime = c.DateTime(nullable: false),
                        CoinAccount_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ProcessId)
                .ForeignKey("dbo.CoinAccounts", t => t.CoinAccount_UserId)
                .Index(t => t.CoinAccount_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountProcesses", "CoinAccount_UserId", "dbo.CoinAccounts");
            DropIndex("dbo.AccountProcesses", new[] { "CoinAccount_UserId" });
            DropTable("dbo.AccountProcesses");
        }
    }
}
