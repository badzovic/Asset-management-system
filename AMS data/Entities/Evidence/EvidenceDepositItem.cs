using AMS_data.Entities.Lookups;
using AMS_data.Entities.Weapons;

namespace AMS_data.Entities.Evidence
{
    public class EvidenceDepositItem
    {
        public int Id { get; set; }

        public int EvidenceDepositId { get; set; }
        public EvidenceDeposit EvidenceDeposit { get; set; } = null!;

        public string? Description { get; set; }

        public int? WeaponItemTypeLookupId { get; set; }
        public LookupItem? WeaponItemTypeLookup { get; set; }

        public int? EvidenceWeaponTypeLookupId { get; set; }
        public LookupItem? EvidenceWeaponTypeLookup { get; set; }

        public int? EvidenceWeaponLookupId { get; set; }
        public LookupItem? EvidenceWeaponLookup { get; set; }

        public int? WeaponLegalityLookupId { get; set; }
        public LookupItem? WeaponLegalityLookup { get; set; }

        public int? LinkedWeaponId { get; set; }
        public Weapon? LinkedWeapon { get; set; }

        public decimal? Quantity { get; set; }
        public string? Unit { get; set; }

        public string? SerialNo { get; set; }
        public string? MarkingText { get; set; }

        public string Status { get; set; } = "Deposited";

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}