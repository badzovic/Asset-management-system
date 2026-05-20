using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Ime { get; set; } = string.Empty;

        [Required]
        public string Prezime { get; set; } = string.Empty;

        public string RoleName { get; set; } = string.Empty;

        public int? OrganizacionaJedinicaId { get; set; }

        public int? SkladisteId { get; set; }

        public bool Aktivan { get; set; }
        public List<string> SelectedRoles { get; set; } = new();

        public List<SelectListItem> Roles { get; set; } = new();
        public List<SelectListItem> OrganizacioneJedinice { get; set; } = new();
        public List<SelectListItem> Skladista { get; set; } = new();
    }
}