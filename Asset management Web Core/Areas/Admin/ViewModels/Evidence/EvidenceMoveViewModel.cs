using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels.Evidence
{
    public class EvidenceMoveViewModel
    {
        public int EvidenceDepositId { get; set; }

        public int? EvidenceDepositItemId { get; set; }

        public int? FromLocationLookupId { get; set; }

        public int? ToLocationLookupId { get; set; }

        public int? MovePurposeLookupId { get; set; }

        public DateTime MoveDate { get; set; } = DateTime.Now;

        public string? ApprovedBy { get; set; }

        public string? MovedBy { get; set; }

        public string? Remarks { get; set; }

        public List<SelectListItem> Locations { get; set; } = new();

        public List<SelectListItem> MovePurposes { get; set; } = new();

        public List<EvidenceMoveHistoryRowVM> History { get; set; } = new();
    }

    public class EvidenceMoveHistoryRowVM
    {
        public int Id { get; set; }

        public string? Item { get; set; }

        public string? FromLocation { get; set; }

        public string? ToLocation { get; set; }

        public string? Purpose { get; set; }

        public DateTime MoveDate { get; set; }

        public string? ApprovedBy { get; set; }

        public string? MovedBy { get; set; }
    }
}