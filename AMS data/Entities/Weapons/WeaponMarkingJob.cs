using AMS_data.Entities.Lookups;

namespace AMS_data.Entities.Weapons
{
    public class WeaponMarkingJob
    {
        public int Id { get; set; }

        public int WeaponId { get; set; }
        public Weapon Weapon { get; set; } = null!;

        public int? MarkingLayoutId { get; set; }
        public MarkingLayout? MarkingLayout { get; set; }

        public DateTime JobDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Prepared";
        // Prepared, SentToMarker, Completed, Failed, Cancelled

        public string? RegistrationNo { get; set; }
        public string? FactorySerial { get; set; }
        public string? WeaponModel { get; set; }
        public string? WeaponType { get; set; }
        public string? Manufacturer { get; set; }
        public string? Caliber { get; set; }

        public string? MarkingText1 { get; set; }
        public string? MarkingText2 { get; set; }
        public string? MarkingText3 { get; set; }

        public string? DataMatrixValue { get; set; }
        public string? QrValue { get; set; }

        public string? OutputJson { get; set; }

        public DateTime? SentAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public string? ErrorMessage { get; set; }

        public string? CreatedByUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }
    }
}