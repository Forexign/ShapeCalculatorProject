namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PermRoleShape : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoleShapePermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.String(maxLength: 128),
                        ShapeType = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleShapePermissions", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.RoleShapePermissions", new[] { "RoleId" });
            DropTable("dbo.RoleShapePermissions");
        }
    }
}
