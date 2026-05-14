using AMS_data.Entities.Weapons;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels.Weapons
{
    public class WeaponMoveViewModel
    {
        public int? SelectedWeaponId { get; set; }

        public Weapon? SelectedWeapon { get; set; }

        [Required]
        public DateTime MoveDate { get; set; } = DateTime.Today;

        [Required]
        public int? MovementActionLookupId { get; set; }

        [Required]
        public int? NewLocationLookupId { get; set; }

        public string? OrderNo { get; set; }

        public string? AuthMoveNo { get; set; }

        public string? MoveOrdinalNo { get; set; } = "001";

        public string? EndUserCertificate { get; set; }

        public string? UserOrgName { get; set; }

        public string? Notes { get; set; }

        public string? AuthorisedByName { get; set; }

        public string? SearchTerm { get; set; }

        public List<SelectListItem> MovementActions { get; set; } = new();

        public List<SelectListItem> NewLocations { get; set; } = new();

        public List<Weapon> ActiveWeapons { get; set; } = new();

        public List<WeaponMove> MoveHistory { get; set; } = new();
    }
}