using AMS_data;
using AMS_data.Entities.Lookups;
using Microsoft.EntityFrameworkCore;

namespace Asset_management_Web_Core.Seeders
{
    public static class LookupSeeder
    {
        public static async Task SeedLookupCategoriesAsync(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetRequiredService<ApplicationDbContext>();

            var categories = new List<LookupCategory>
            {
                new() { Key = "MarkLocation", Name = "Mark Location" },
                new() { Key = "Country", Name = "Country" },
                new() { Key = "Region", Name = "Region" },
                new() { Key = "GovernmentAgency", Name = "Government Agency" },
                new() { Key = "OriginalLocation", Name = "Original Location" },
                new() { Key = "OriginIndicator", Name = "Origin Indicator" },
                new() { Key = "OriginalState", Name = "Original State" },
                new() { Key = "Unit", Name = "Unit" },
                new() { Key = "Stock", Name = "Stock" },
                new() { Key = "BookkeepingBy", Name = "Bookkeeping By" },
                new() { Key = "ManufacturerCountry", Name = "Manufacturer Country" },
                new() { Key = "IdType", Name = "ID Type" },
                new() { Key = "MovementAction", Name = "Movement Action" },
                new() { Key = "MovementReason", Name = "Movement Reason" },
                new() { Key = "Location", Name = "Location" },

                new() { Key = "WeaponState", Name = "Weapon State" },
                new() { Key = "WeaponLegality", Name = "Weapon Legality" },
                new() { Key = "WeaponItemType", Name = "Weapon Item Type" },
                new() { Key = "ManufactureYear", Name = "Manufacture Year" },
                new() { Key = "PurposeOfUse", Name = "Purpose Of Use" },
                new() { Key = "CheckState", Name = "Check State" },

                new() { Key = "MoveStatus", Name = "Move Status" },
                new() { Key = "MovedWeaponLocation", Name = "Moved Weapon Location" },
                new() { Key = "MoveAuthoriserNames", Name = "Move Authoriser Names" },
                new() { Key = "MoveEvidencePurpose", Name = "Move Evidence Purpose" },

                new() { Key = "SearchOptions", Name = "Search Options" },

                new() { Key = "EvidenceWeaponType", Name = "Evidence Weapon Type" },
                new() { Key = "EvidenceWeapon", Name = "Evidence Weapon" },
                new() { Key = "EvidenceIndicator", Name = "Evidence Indicator" },
                new() { Key = "EvidenceDepositLocation", Name = "Evidence Deposit Location" },

                new() { Key = "CaseType", Name = "Case Type" },
                new() { Key = "AgeBand", Name = "Age Band" },
                new() { Key = "Sex", Name = "Sex" }
            };

            foreach (var category in categories)
            {
                var exists = await db.LookupCategories
                    .AnyAsync(x => x.Key == category.Key);

                if (!exists)
                {
                    db.LookupCategories.Add(category);
                }
            }

            await db.SaveChangesAsync();

            await SeedLookupItemsAsync(db);
        }

        private static async Task SeedLookupItemsAsync(ApplicationDbContext db)
        {
            await SeedItems(db, "Country", new[]
            {
                "Bosnia and Herzegovina",
                "Croatia",
                "Serbia",
                "Germany",
                "Austria",
                "USA"
            });

            await SeedItems(db, "Region", new[]
            {
                "Sarajevo",
                "Mostar",
                "Banja Luka",
                "Tuzla",
                "Zenica"
            });

            await SeedItems(db, "GovernmentAgency", new[]
            {
                "Federal Police Administration",
                "SIPA",
                "Border Police",
                "Ministry of Defence",
                "Special Police Unit"
            });

            await SeedItems(db, "OriginIndicator", new[]
            {
                "Purchase",
                "Donation",
                "Transfer",
                "Unknown",
                "Confiscated"
            });

            await SeedItems(db, "Stock", new[]
            {
                "Service Weapons",
                "Reserve Weapons",
                "Museum Exhibit",
                "Disposal",
                "Training Weapons"
            });

            await SeedItems(db, "WeaponState", new[]
            {
                "Functional",
                "Damaged",
                "Under Repair",
                "Deactivated",
                "Destroyed"
            });

            await SeedItems(db, "WeaponLegality", new[]
            {
                "Legal",
                "Illegal",
                "Under Investigation"
            });

            await SeedItems(db, "MoveStatus", new[]
            {
                "Pending",
                "Approved",
                "Rejected",
                "Completed"
            });

            await SeedItems(db, "MovementReason", new[]
            {
                "Export",
                "Transfer",
                "Repair",
                "Deactivation",
                "Training"
            });

            await SeedItems(db, "Location", new[]
            {
                "Sarajevo HQ",
                "Mostar Warehouse",
                "Tuzla Armory",
                "Rajlovac Depot"
            });

            await SeedItems(db, "MarkLocation", new[]
            {
                "Frame Right",
                "Frame Left",
                "Receiver",
                "Barrel",
                "Slide"
            });

            await SeedItems(db, "CheckState", new[]
            {
                "Passed",
                "Failed",
                "Needs Inspection"
            });

            await SeedItems(db, "Sex", new[]
            {
                "Male",
                "Female"
            });
        }

        private static async Task SeedItems(
            ApplicationDbContext db,
            string categoryKey,
            IEnumerable<string> items)
        {
            var category = await db.LookupCategories
                .FirstOrDefaultAsync(x => x.Key == categoryKey);

            if (category == null)
                return;

            var order = 1;

            foreach (var item in items)
            {
                var exists = await db.LookupItems.AnyAsync(x =>
                    x.LookupCategoryId == category.Id &&
                    x.Name == item);

                if (!exists)
                {
                    db.LookupItems.Add(new LookupItem
                    {
                        LookupCategoryId = category.Id,
                        Name = item,
                        DisplayOrder = order,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                order++;
            }

            await db.SaveChangesAsync();
        }
    }
}