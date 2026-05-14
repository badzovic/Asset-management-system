using AMS_data.Entities.Weapons;
using System.ComponentModel.DataAnnotations;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels.Weapons
{
    public class WeaponMoveAuthoriseViewModel
    {
        public int? SelectedMoveId { get; set; }

        public WeaponMove? SelectedMove { get; set; }

        [Required]
        public string AuthorisedByName { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public List<WeaponMove> PreparedMoves { get; set; } = new();

        public List<WeaponMove> AuthorisedMoves { get; set; } = new();
    }
}