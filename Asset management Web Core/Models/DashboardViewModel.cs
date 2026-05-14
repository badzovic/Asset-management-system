using AMS_data.Entities.Weapons;

namespace Asset_management_Web_Core.Models
{
    public class DashboardViewModel
    {
        public int TotalWeapons { get; set; }
        public int MarkedWeapons { get; set; }
        public int UnmarkedWeapons { get; set; }
        public int TotalChecks { get; set; }

        public List<Weapon> RecentWeapons { get; set; } = new();
        public List<WeaponCheck> RecentChecks { get; set; } = new();
    }
}