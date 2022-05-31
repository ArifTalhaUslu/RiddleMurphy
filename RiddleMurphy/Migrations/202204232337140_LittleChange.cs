namespace RiddleMurphy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LittleChange : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Follows");
            AlterColumn("dbo.Follows", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Follows", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Follows");
            AlterColumn("dbo.Follows", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Follows", "Id");
        }
    }
}
