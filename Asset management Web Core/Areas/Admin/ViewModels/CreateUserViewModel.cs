using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        public string? Ime { get; set; }

        public string? Prezime { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string RoleName { get; set; } = string.Empty;

        public int? OrganizacionaJedinicaId { get; set; }

        public int? SkladisteId { get; set; }

        public List<SelectListItem> Roles { get; set; } = new();

        public List<SelectListItem> OrganizacioneJedinice { get; set; } = new();

        public List<SelectListItem> Skladista { get; set; } = new();

        public List<UserListItemViewModel> Users { get; set; } = new();
    }
}