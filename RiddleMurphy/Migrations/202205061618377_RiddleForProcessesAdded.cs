namespace RiddleMurphy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RiddleForProcessesAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountProcesses", "RiddleId_RiddleId", c => c.Int());
            CreateIndex("dbo.AccountProcesses", "RiddleId_RiddleId");
            AddForeignKey("dbo.AccountProcesses", "RiddleId_RiddleId", "dbo.Riddles", "RiddleId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountProcesses", "RiddleId_RiddleId", "dbo.Riddles");
            DropIndex("dbo.AccountProcesses", new[] { "RiddleId_RiddleId" });
            DropColumn("dbo.AccountProcesses", "RiddleId_RiddleId");
        }
    }
}
