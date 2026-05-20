using AMS_data.Entities.Lookups;

namespace AMS_data.Entities.Narcotics
{
    public class NarcoticsDepositItem
    {
        public int Id { get; set; }

        public int NarcoticsDepositId { get; set; }
        public NarcoticsDeposit NarcoticsDeposit { get; set; } = null!;

        public int? NarcoticsTypeLookupId { get; set; }
        public LookupItem? NarcoticsTypeLookup { get; set; }

        public decimal? Quantity { get; set; }

        public int? QuantityUnitLookupId { get; set; }
        public LookupItem? QuantityUnitLookup { get; set; }

        public int? CompositionLookupId { get; set; }
        public LookupItem? CompositionLookup { get; set; }

        public string Status { get; set; } = "Deposited";

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}