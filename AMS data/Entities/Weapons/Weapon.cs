using AMS_data.Entities.Lookups;

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

        public int? MarkLocationLookupId { get; set; }
        public LookupItem? MarkLocationLookup { get; set; }

        public int? CountryLookupId { get; set; }
        public LookupItem? CountryLookup { get; set; }

        public int? RegionLookupId { get; set; }
        public LookupItem? RegionLookup { get; set; }

        public int? GovernmentAgencyLookupId { get; set; }
        public LookupItem? GovernmentAgencyLookup { get; set; }

        public int? ManufactureCountryLookupId { get; set; }
        public LookupItem? ManufactureCountryLookup { get; set; }

        public DateTime? ManufactureDate { get; set; }

        public int? OriginalLocationLookupId { get; set; }
        public LookupItem? OriginalLocationLookup { get; set; }

        public int? OriginIndicatorLookupId { get; set; }
        public LookupItem? OriginIndicatorLookup { get; set; }

        public int? OriginalStateLookupId { get; set; }
        public LookupItem? OriginalStateLookup { get; set; }

        public int? UnitLookupId { get; set; }
        public LookupItem? UnitLookup { get; set; }

        public int? StockLookupId { get; set; }
        public LookupItem? StockLookup { get; set; }

        public int? BookkeepingByLookupId { get; set; }
        public LookupItem? BookkeepingByLookup { get; set; }

        public string? BarrelMark { get; set; }
        public string? SlideMark { get; set; }
        public string? ButtstockMark { get; set; }

        public int? IdTypeLookupId { get; set; }
        public LookupItem? IdTypeLookup { get; set; }

        public string? IdNo { get; set; }
        public string? HolderInfo { get; set; }
        public DateTime? DateOfOwnership { get; set; }

        public string? Notes { get; set; }
        public string? InventoryNo { get; set; }

        public bool TempStock { get; set; }

        public DateTime? DonationDate { get; set; }

        public int? DonorAgencyLookupId { get; set; }
        public LookupItem? DonorAgencyLookup { get; set; }

        public string? DonorContractNo { get; set; }

        public int? CurrentStatusId { get; set; }
        public WeaponStatus? CurrentStatus { get; set; }

        public int? WeaponStateLookupId { get; set; }
        public LookupItem? WeaponStateLookup { get; set; }

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