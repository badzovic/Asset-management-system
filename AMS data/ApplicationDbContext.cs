using AMS_data.Entities;
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
    }
}