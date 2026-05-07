using Microsoft.AspNetCore.Identity;

namespace AMS_data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? Ime { get; set; }
        public string? Prezime { get; set; }

        public int? OrganizacionaJedinicaId { get; set; }
        public int? SkladisteId { get; set; }

        public bool Aktivan { get; set; } = true;
        public DateTime DatumKreiranja { get; set; } = DateTime.Now;
    }
}