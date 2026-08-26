using AMS_data.Entities;
using AMS_data.Entities.Evidence;
using AMS_data.Entities.Lookups;
using AMS_data.Entities.Narcotics;
using AMS_data.Entities.Weapons;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<EvidenceDeposit> EvidenceDeposits { get; set; }
        public DbSet<EvidenceDepositItem> EvidenceDepositItems { get; set; }
        public DbSet<EvidenceMoveHistory> EvidenceMoveHistories { get; set; }
        public DbSet<NarcoticsDeposit> NarcoticsDeposits { get; set; }
        public DbSet<NarcoticsDepositItem> NarcoticsDepositItems { get; set; }
        public DbSet<NarcoticsMoveHistory> NarcoticsMoveHistories { get; set; }
        public DbSet<LaserJob> LaserJobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LaserJob>(entity =>
            {
                entity.ToTable("LASER_JOBS");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.WeaponId).HasColumnName("WEAPON_ID");
                entity.Property(e => e.LayoutCode).HasColumnName("LAYOUT_CODE").HasMaxLength(100).IsRequired();
                entity.Property(e => e.RegistrationNo).HasColumnName("REGISTRATION_NO").HasMaxLength(100);
                entity.Property(e => e.FactorySerial).HasColumnName("FACTORY_SERIAL").HasMaxLength(100);
                entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(30).IsRequired();
                entity.Property(e => e.CreatedOn).HasColumnName("CREATED_ON");
                entity.Property(e => e.StartedOn).HasColumnName("STARTED_ON");
                entity.Property(e => e.MarkedOn).HasColumnName("MARKED_ON");
                entity.Property(e => e.ErrorMessage).HasColumnName("ERROR_MESSAGE").HasMaxLength(1000);

                entity.HasOne(e => e.Weapon)
                    .WithMany()
                    .HasForeignKey(e => e.WeaponId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}