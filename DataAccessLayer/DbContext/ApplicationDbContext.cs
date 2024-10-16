using DataAccessLayer.FormulaModels;
using DataAccessLayer.Migrations;
using DataAccessLayer.Models;
using Entities.ShapeModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoleActionPermission = DataAccessLayer.Models.RoleActionPermission;

namespace DataAccessLayer.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("NewConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
        }
        public DbSet<Inputs> Inputs { get; set; }
        public DbSet<ShapeResult> ShapeResults { get; set; }
        public DbSet<ApiLog> ApiLogs { get; set; }
        public DbSet<Api> Apis { get; set; }
        public DbSet<IdentityUserRole> UserRoles { get; set; }
        public DbSet<ShapeType> ShapeTypes { get; set; }
        public DbSet<RoleShapePermission> RoleShapePermissions { get; set; }
        public DbSet<RoleActionPermission> RoleActionPermissions { get; set; }
        public DbSet<Formula> Formulas { get; set; }
        public DbSet<FormulaInput> FormulaInputs { get; set; }
        public DbSet<FormulaParameter> FormulaParameters { get; set; }
        public DbSet<FormulaParameterInput> FormulaParameterInputs { get; set; }
        public DbSet<FormulaResult> FormulaResults { get; set; }
    }
}
