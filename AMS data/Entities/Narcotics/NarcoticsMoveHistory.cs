using AMS_data.Entities.Lookups;

namespace AMS_data.Entities.Narcotics
{
    public class NarcoticsMoveHistory
    {
        public int Id { get; set; }

        public int NarcoticsDepositId { get; set; }
        public NarcoticsDeposit NarcoticsDeposit { get; set; } = null!;

        public int? NarcoticsDepositItemId { get; set; }
        public NarcoticsDepositItem? NarcoticsDepositItem { get; set; }

        public int? FromLocationLookupId { get; set; }
        public LookupItem? FromLocationLookup { get; set; }

        public int? ToLocationLookupId { get; set; }
        public LookupItem? ToLocationLookup { get; set; }

        public int? MovePurposeLookupId { get; set; }
        public LookupItem? MovePurposeLookup { get; set; }

        public DateTime MoveDate { get; set; } = DateTime.UtcNow;

        public string? ApprovedBy { get; set; }
        public string? MovedBy { get; set; }

        public string? Remarks { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}