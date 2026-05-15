namespace AMS_data.Entities.Weapons
{
    public class MarkingLayoutObject
    {
        public int Id { get; set; }

        public int MarkingLayoutId { get; set; }
        public MarkingLayout MarkingLayout { get; set; } = null!;

        public string ObjectType { get; set; } = "Text";
        // Text, VariableText, DataMatrix, QRCode, Line, Rectangle, Image, DxfReference

        public string Name { get; set; } = string.Empty;

        public decimal X { get; set; }
        public decimal Y { get; set; }

        public decimal Width { get; set; } = 20;
        public decimal Height { get; set; } = 8;

        public decimal Rotation { get; set; }

        public string? TextValue { get; set; }

        public string? VariableName { get; set; }

        public decimal FontSize { get; set; } = 4;

        public bool IsBold { get; set; }

        public decimal StrokeWidth { get; set; } = 0.2M;

        public string? PropertiesJson { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;
    }
}