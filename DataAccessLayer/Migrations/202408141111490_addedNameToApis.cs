namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedNameToApis : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apis", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Apis", "Name");
        }
    }
}
