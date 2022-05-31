namespace RiddleMurphy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SolwerNamePropAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Riddles", "SolwerName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Riddles", "SolwerName");
        }
    }
}
