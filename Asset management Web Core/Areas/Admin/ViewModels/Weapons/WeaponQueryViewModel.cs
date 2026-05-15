using AMS_data.Entities.Weapons;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels.Weapons
{
    public class WeaponQueryViewModel
    {
        public string? RegistrationNo { get; set; }
        public string? FactorySerial { get; set; }

        public int? WeaponTypeId { get; set; }
        public int? WeaponModelId { get; set; }
        public int? ManufacturerId { get; set; }
        public int? CaliberId { get; set; }

        public int? CountryLookupId { get; set; }
        public int? RegionLookupId { get; set; }
        public int? OriginalLocationLookupId { get; set; }
        public int? OriginIndicatorLookupId { get; set; }
        public int? StockLookupId { get; set; }
        public int? UnitLookupId { get; set; }

        public bool? IsMarked { get; set; }
        public bool? IsProspective { get; set; }

        public DateTime? RegistrationDateFrom { get; set; }
        public DateTime? RegistrationDateTo { get; set; }

        public bool ShowRegistrationNo { get; set; } = true;
        public bool ShowFactorySerial { get; set; } = true;
        public bool ShowModel { get; set; } = true;
        public bool ShowType { get; set; } = true;
        public bool ShowManufacturer { get; set; } = true;
        public bool ShowCaliber { get; set; } = true;
        public bool ShowRegistrationDate { get; set; } = true;
        public bool ShowMarked { get; set; } = true;
        public bool ShowLocation { get; set; } = true;

        public List<SelectListItem> WeaponTypes { get; set; } = new();
        public List<SelectListItem> WeaponModels { get; set; } = new();
        public List<SelectListItem> Manufacturers { get; set; } = new();
        public List<SelectListItem> Calibers { get; set; } = new();

        public List<SelectListItem> Countries { get; set; } = new();
        public List<SelectListItem> Regions { get; set; } = new();
        public List<SelectListItem> OriginalLocations { get; set; } = new();
        public List<SelectListItem> OriginIndicators { get; set; } = new();
        public List<SelectListItem> Stocks { get; set; } = new();
        public List<SelectListItem> Units { get; set; } = new();
        public List<SavedWeaponQuery> SavedQueries { get; set; } = new();
        public List<Weapon> Results { get; set; } = new();
        public bool HasSearched { get; set; }
    }
}