using AMS_data.Entities.Lookups;

namespace AMS_data.Entities.Weapons
{
    public class WeaponMove
    {
        public int Id { get; set; }

        public int WeaponId { get; set; }
        public Weapon Weapon { get; set; } = null!;

        public DateTime MoveDate { get; set; } = DateTime.UtcNow;

        public int? MovementActionLookupId { get; set; }
        public LookupItem? MovementActionLookup { get; set; }

        public int? NewLocationLookupId { get; set; }
        public LookupItem? NewLocationLookup { get; set; }

        public string? OrderNo { get; set; }
        public string? AuthMoveNo { get; set; }
        public string? MoveOrdinalNo { get; set; }
        public string? EndUserCertificate { get; set; }
        public string? UserOrgName { get; set; }
        public string? Notes { get; set; }

        public string Status { get; set; } = "Prepared";

        public string? PreparedByUserId { get; set; }
        public DateTime PreparedAt { get; set; } = DateTime.UtcNow;

        public string? AuthorisedByName { get; set; }
        public string? AuthorisedByUserId { get; set; }
        public DateTime? AuthorisedAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}