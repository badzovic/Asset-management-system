using AMS_data.Entities.Weapons;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels.Weapons
{
    public class WeaponMarkingViewModel
    {
        public int? SelectedWeaponId { get; set; }

        public Weapon? SelectedWeapon { get; set; }

        [Required]
        public int? MarkingLayoutId { get; set; }

        public string? MarkingText1 { get; set; }
        public string? MarkingText2 { get; set; }
        public string? MarkingText3 { get; set; }

        public string? DataMatrixValue { get; set; }
        public string? QrValue { get; set; }

        public List<SelectListItem> MarkingLayouts { get; set; } = new();

        public List<Weapon> Weapons { get; set; } = new();

        public List<WeaponMarkingJob> MarkingHistory { get; set; } = new();
    }
}