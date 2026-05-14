using AMS_data.Entities.Weapons;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels.Weapons
{
    public class WeaponCheckViewModel
    {
        public int? SelectedWeaponId { get; set; }

        public Weapon? SelectedWeapon { get; set; }

        [Required]
        public DateTime CheckDate { get; set; } = DateTime.Today;

        [Required]
        public int? CheckStateLookupId { get; set; }

        public string? IdNo { get; set; }

        public string? Comments { get; set; }

        public string? SearchTerm { get; set; }

        public List<SelectListItem> CheckStates { get; set; } = new();

        public List<Weapon> ActiveWeapons { get; set; } = new();

        public List<WeaponCheck> CheckHistory { get; set; } = new();
    }
}