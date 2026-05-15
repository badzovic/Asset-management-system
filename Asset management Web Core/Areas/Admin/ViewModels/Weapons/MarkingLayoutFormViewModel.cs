using System.ComponentModel.DataAnnotations;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels.Weapons
{
    public class MarkingLayoutFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string LayoutType { get; set; } = "Laser";

        public string? Description { get; set; }

        public decimal WidthMm { get; set; } = 100;
        public decimal HeightMm { get; set; } = 80;

        public string Unit { get; set; } = "mm";

        public string? TemplateFilePath { get; set; }
        public string? BackgroundFilePath { get; set; }
        public string? PreviewImagePath { get; set; }

        public IFormFile? TemplateFile { get; set; }
        public IFormFile? BackgroundFile { get; set; }
        public IFormFile? PreviewImage { get; set; }

        public bool IsActive { get; set; } = true;
    }
}