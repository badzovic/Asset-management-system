using AMS_data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AMS_data.Entities.Weapons;
using AMS_data.Entities.Lookups;

namespace AMS_data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<OrganizacionaJedinica> OrganizacioneJedinice { get; set; }
        public DbSet<Skladiste> Skladista { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<WeaponType> WeaponTypes { get; set; }
        public DbSet<Caliber> Calibers { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<WeaponStatus> WeaponStatuses { get; set; }
        public DbSet<WeaponModel> WeaponModels { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<LookupCategory> LookupCategories { get; set; }
        public DbSet<LookupItem> LookupItems { get; set; }
        public DbSet<WeaponCheck> WeaponChecks { get; set; }
        public DbSet<WeaponMove> WeaponMoves { get; set; }
        public DbSet<SavedWeaponQuery> SavedWeaponQueries { get; set; }
        public DbSet<MarkingLayout> MarkingLayouts { get; set; }
        public DbSet<WeaponMarkingJob> WeaponMarkingJobs { get; set; }
        public DbSet<MarkingLayoutObject> MarkingLayoutObjects { get; set; }
    }
}