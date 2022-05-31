namespace RiddleMurphy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserBioAndRiddleRejectPropAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserBio", c => c.String(maxLength: 200, unicode: false));
            AddColumn("dbo.Riddles", "isRiddleRejected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Riddles", "isRiddleRejected");
            DropColumn("dbo.Users", "UserBio");
        }
    }
}
