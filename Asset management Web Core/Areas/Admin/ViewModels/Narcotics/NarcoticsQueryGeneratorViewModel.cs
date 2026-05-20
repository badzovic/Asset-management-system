using AMS_data.Entities.Narcotics;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels.Narcotics
{
    public class NarcoticsQueryGeneratorViewModel
    {
        public string? RegistrationNo { get; set; }
        public string? CaseNo { get; set; }

        public int? CaseTypeLookupId { get; set; }
        public int? DepositLocationLookupId { get; set; }
        public int? OUPerformedSeizureLookupId { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public bool ShowRegistrationNo { get; set; } = true;
        public bool ShowCaseNo { get; set; } = true;
        public bool ShowCaseType { get; set; } = true;
        public bool ShowLocation { get; set; } = true;
        public bool ShowOU { get; set; } = true;
        public bool ShowStatus { get; set; } = true;
        public bool ShowCreatedAt { get; set; } = true;

        public bool HasSearched { get; set; }

        public List<SelectListItem> CaseTypes { get; set; } = new();
        public List<SelectListItem> DepositLocations { get; set; } = new();
        public List<SelectListItem> OUList { get; set; } = new();

        public List<NarcoticsDeposit> Results { get; set; } = new();
    }
}