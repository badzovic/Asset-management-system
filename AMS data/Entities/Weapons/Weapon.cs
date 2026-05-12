using AMS_data.Entities;

namespace AMS_data.Entities.Weapons
{
    public class Weapon
    {
        public int Id { get; set; }

        public string RegistrationNo { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public string? FactorySerial { get; set; }
        public string? ConfirmSerial { get; set; }

        public int? WeaponTypeId { get; set; }
        public WeaponType? WeaponType { get; set; }

        public int? WeaponModelId { get; set; }
        public WeaponModel? WeaponModel { get; set; }

        public int? CaliberId { get; set; }
        public Caliber? Caliber { get; set; }

        public int? ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }

        public string? MarkLocation { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? GovernmentAgency { get; set; }

        public string? OriginalLocation { get; set; }
        public string? OriginIndicator { get; set; }
        public string? OriginalState { get; set; }

        public int? OrganizacionaJedinicaId { get; set; }
        public OrganizacionaJedinica? OrganizacionaJedinica { get; set; }

        public int? SkladisteId { get; set; }
        public Skladiste? Skladiste { get; set; }

        public string? BookkeepingBy { get; set; }

        public string? BarrelMark { get; set; }
        public string? SlideMark { get; set; }
        public string? ButtstockMark { get; set; }

        public string? IdNo { get; set; }
        public string? HolderInfo { get; set; }
        public DateTime? DateOfOwnership { get; set; }

        public string? Notes { get; set; }
        public string? InventoryNo { get; set; }

        public bool TempStock { get; set; }
        public DateTime? DonationDate { get; set; }
        public string? DonorAgency { get; set; }
        public string? DonorContractNo { get; set; }

        public int? CurrentStatusId { get; set; }
        public WeaponStatus? CurrentStatus { get; set; }

        public string? FunctionalStatus { get; set; }

        public bool IsMarked { get; set; }
        public bool IsProspective { get; set; }

        public string? ImagePath { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}