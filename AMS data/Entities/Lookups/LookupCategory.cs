using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_data.Entities.Lookups
{
    public class LookupCategory
    {
        public int Id { get; set; }

        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<LookupItem> Items { get; set; } = new List<LookupItem>();
    }
}