using AMS_data.Entities.Evidence;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Asset_management_Web_Core.Areas.Admin.ViewModels.Evidence
{
    public class EvidenceQueryGeneratorViewModel
    {
        public string? RegistrationNo { get; set; }
        public string? CaseNo { get; set; }

        public int? CaseTypeLookupId { get; set; }
        public int? DepositLocationLookupId { get; set; }

        public DateTime? RegistrationDateFrom { get; set; }
        public DateTime? RegistrationDateTo { get; set; }

        public bool ShowRegistrationNo { get; set; } = true;
        public bool ShowCaseNo { get; set; } = true;
        public bool ShowCaseType { get; set; } = true;
        public bool ShowRegistrationDate { get; set; } = true;
        public bool ShowReceivedDate { get; set; }
        public bool ShowDepositLocation { get; set; } = true;
        public bool ShowEvidenceIndicator { get; set; }
        public bool ShowFirstName { get; set; }
        public bool ShowSurname { get; set; }
        public bool ShowPersonalIdNo { get; set; }
        public bool ShowStatus { get; set; } = true;

        public bool HasSearched { get; set; }

        public List<SelectListItem> CaseTypes { get; set; } = new();
        public List<SelectListItem> DepositLocations { get; set; } = new();

        public List<EvidenceDeposit> Results { get; set; } = new();
    }
}