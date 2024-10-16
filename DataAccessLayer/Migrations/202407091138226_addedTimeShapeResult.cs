namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedTimeShapeResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShapeResults", "CreatedTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShapeResults", "CreatedTime");
        }
    }
}
