namespace RiddleMurphy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LittleChangeForName : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AccountProcesses", name: "RiddleId_RiddleId", newName: "Riddle_RiddleId");
            RenameIndex(table: "dbo.AccountProcesses", name: "IX_RiddleId_RiddleId", newName: "IX_Riddle_RiddleId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AccountProcesses", name: "IX_Riddle_RiddleId", newName: "IX_RiddleId_RiddleId");
            RenameColumn(table: "dbo.AccountProcesses", name: "Riddle_RiddleId", newName: "RiddleId_RiddleId");
        }
    }
}
