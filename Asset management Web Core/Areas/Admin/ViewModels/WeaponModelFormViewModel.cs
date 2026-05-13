using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels
{
    public class WeaponModelFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Code { get; set; }
        public string? Description { get; set; }

        public int? WeaponTypeId { get; set; }
        public int? ManufacturerId { get; set; }
        public int? CaliberId { get; set; }

        public string? ExistingImagePath { get; set; }

        public IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; } = true;

        public List<SelectListItem> WeaponTypes { get; set; } = new();
        public List<SelectListItem> Manufacturers { get; set; } = new();
        public List<SelectListItem> Calibers { get; set; } = new();
    }
}