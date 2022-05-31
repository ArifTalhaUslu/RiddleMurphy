namespace RiddleMurphy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SolwerIdPropertyAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Riddles", "IsRiddleAnswered", c => c.Boolean(nullable: false));
            AddColumn("dbo.Riddles", "SolwerId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Riddles", "SolwerId");
            DropColumn("dbo.Riddles", "IsRiddleAnswered");
        }
    }
}
