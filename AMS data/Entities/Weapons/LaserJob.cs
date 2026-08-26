using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_data.Entities.Weapons
{
    public class LaserJob
    {
        public int Id { get; set; }
        public int WeaponId { get; set; }
        public string LayoutCode { get; set; } = null!;
        public string? RegistrationNo { get; set; }
        public string? FactorySerial { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public DateTime? StartedOn { get; set; }
        public DateTime? MarkedOn { get; set; }
        public string? ErrorMessage { get; set; }

        public virtual Weapon Weapon { get; set; } = null!;
    }
}