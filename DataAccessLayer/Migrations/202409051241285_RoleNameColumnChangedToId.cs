namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleNameColumnChangedToId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoleActionPermissions", "RoleId", c => c.String());
            DropColumn("dbo.RoleActionPermissions", "RoleName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RoleActionPermissions", "RoleName", c => c.String());
            DropColumn("dbo.RoleActionPermissions", "RoleId");
        }
    }
}
