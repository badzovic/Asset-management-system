using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_data.Entities.Weapons
{
    public class WeaponModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string? Description { get; set; }

        public int? WeaponTypeId { get; set; }
        public WeaponType? WeaponType { get; set; }

        public int? ManufacturerId { get; set; }
        public Manufacturer? Manufacturer { get; set; }

        public int? CaliberId { get; set; }
        public Caliber? Caliber { get; set; }

        public string? ImagePath { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}