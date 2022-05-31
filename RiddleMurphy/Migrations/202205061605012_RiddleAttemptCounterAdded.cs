namespace RiddleMurphy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RiddleAttemptCounterAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Riddles", "AttemptCounter", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Riddles", "AttemptCounter");
        }
    }
}
