namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingUserIdToTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inputs", "UserId", c => c.Guid(nullable: false));
            AddColumn("dbo.ShapeResults", "UserId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShapeResults", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Inputs", "UserId", c => c.Int(nullable: false));
        }
    }
}
