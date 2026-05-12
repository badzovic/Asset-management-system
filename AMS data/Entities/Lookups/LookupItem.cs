using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS_data.Entities.Lookups
{
    public class LookupItem
    {
        public int Id { get; set; }

        public int LookupCategoryId { get; set; }
        public LookupCategory LookupCategory { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string? Description { get; set; }

        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}