namespace RiddleMurphy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DislikePropDeleted : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Riddles", "RiddleDislike");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Riddles", "RiddleDislike", c => c.Int(nullable: false));
        }
    }
}
