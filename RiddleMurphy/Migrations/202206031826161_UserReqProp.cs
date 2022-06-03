namespace RiddleMurphy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserReqProp : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "UserProfileImgPath", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "UserProfileImgPath", c => c.String());
        }
    }
}
