using AMS_data.Entities.Lookups;

namespace AMS_data.Entities.Weapons
{
    public class WeaponCheck
    {
        public int Id { get; set; }

        public int WeaponId { get; set; }
        public Weapon Weapon { get; set; } = null!;

        public DateTime CheckDate { get; set; } = DateTime.UtcNow;

        public int? CheckStateLookupId { get; set; }
        public LookupItem? CheckStateLookup { get; set; }

        public string? IdNo { get; set; }

        public string? Comments { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
    }
}