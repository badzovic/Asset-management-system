namespace AMS_data.Entities.Weapons
{
    public class SavedWeaponQuery
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? UserId { get; set; }

        public bool IsPublic { get; set; }

        public string QueryJson { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}