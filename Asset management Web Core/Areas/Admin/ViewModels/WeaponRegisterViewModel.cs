using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels.Weapons
{
    public class WeaponRegisterViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string RegistrationNo { get; set; } = string.Empty;

        [Required]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public bool IsMarked { get; set; }

        public bool IsProspective { get; set; }

        [Required]
        public string? FactorySerial { get; set; }

        [Required]
        public string? ConfirmSerial { get; set; }

        [Required]
        public int? WeaponTypeId { get; set; }

        [Required]
        public int? WeaponModelId { get; set; }

        [Required]
        public int? CaliberId { get; set; }

        [Required]
        public int? ManufacturerId { get; set; }

        public int? MarkLocationLookupId { get; set; }

        public int? CountryLookupId { get; set; }

        public int? RegionLookupId { get; set; }

        public int? GovernmentAgencyLookupId { get; set; }

        public int? ManufactureCountryLookupId { get; set; }

        public DateTime? ManufactureDate { get; set; }

        public int? OriginalLocationLookupId { get; set; }

        public int? OriginIndicatorLookupId { get; set; }

        public int? OriginalStateLookupId { get; set; }

        public int? UnitLookupId { get; set; }

        public int? StockLookupId { get; set; }

        public int? BookkeepingByLookupId { get; set; }

        public string? BarrelMark { get; set; }

        public string? SlideMark { get; set; }

        public string? ButtstockMark { get; set; }

        public int? IdTypeLookupId { get; set; }

        public string? IdNo { get; set; }

        public string? HolderInfo { get; set; }

        public DateTime? DateOfOwnership { get; set; }

        public string? Notes { get; set; }

        public string? InventoryNo { get; set; }

        public bool TempStock { get; set; }

        public DateTime? DonationDate { get; set; }

        public int? DonorAgencyLookupId { get; set; }

        public string? DonorContractNo { get; set; }

        public int? CurrentStatusId { get; set; }

        public int? WeaponStateLookupId { get; set; }

        public IFormFile? ImageFile { get; set; }

        public string? ExistingImagePath { get; set; }

        public List<SelectListItem> WeaponTypes { get; set; } = new();

        public List<SelectListItem> WeaponModels { get; set; } = new();

        public List<SelectListItem> Manufacturers { get; set; } = new();

        public List<SelectListItem> Calibers { get; set; } = new();

        public List<SelectListItem> WeaponStatuses { get; set; } = new();

        public List<SelectListItem> MarkLocations { get; set; } = new();

        public List<SelectListItem> Countries { get; set; } = new();

        public List<SelectListItem> Regions { get; set; } = new();

        public List<SelectListItem> GovernmentAgencies { get; set; } = new();

        public List<SelectListItem> ManufactureCountries { get; set; } = new();

        public List<SelectListItem> OriginalLocations { get; set; } = new();

        public List<SelectListItem> OriginIndicators { get; set; } = new();

        public List<SelectListItem> OriginalStates { get; set; } = new();

        public List<SelectListItem> Units { get; set; } = new();

        public List<SelectListItem> Stocks { get; set; } = new();

        public List<SelectListItem> BookkeepingByList { get; set; } = new();

        public List<SelectListItem> IdTypes { get; set; } = new();

        public List<SelectListItem> DonorAgencies { get; set; } = new();

        public List<SelectListItem> WeaponStates { get; set; } = new();
    }
}