namespace AMS_data.Entities.Weapons
{
    public class MarkingLayout
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string LayoutType { get; set; } = "Laser";
        // Laser, DotPeen, ZebraLabel

        public string? Description { get; set; }

        public decimal WidthMm { get; set; } = 100;
        public decimal HeightMm { get; set; } = 80;

        public string Unit { get; set; } = "mm";

        public string? TemplateFilePath { get; set; }
        public string? BackgroundFilePath { get; set; }
        public string? PreviewImagePath { get; set; }

        public string? LayoutJson { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<MarkingLayoutObject> Objects { get; set; } = new List<MarkingLayoutObject>();
    }
}