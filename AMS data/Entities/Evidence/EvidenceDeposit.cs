using AMS_data.Entities.Lookups;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace AMS_data.Entities.Evidence
{
    public class EvidenceDeposit
    {
        public int Id { get; set; }

        public string RegistrationNo { get; set; } = string.Empty;

        public string? CaseNo { get; set; }

        public int? CaseTypeLookupId { get; set; }
        public LookupItem? CaseTypeLookup { get; set; }

        public int? EvidenceIndicatorLookupId { get; set; }
        public LookupItem? EvidenceIndicatorLookup { get; set; }

        public int? DepositLocationLookupId { get; set; }
        public LookupItem? DepositLocationLookup { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReceivedDate { get; set; }

        public string? StorageOrderNo { get; set; }
        public DateTime? StorageOrderDate { get; set; }

        public string? SubmittedByOfficer { get; set; }
        public string? HandlingOfficer { get; set; }

        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public string? PersonalIdNo { get; set; }

        public int? SexLookupId { get; set; }
        public LookupItem? SexLookup { get; set; }

        public int? AgeBandLookupId { get; set; }
        public LookupItem? AgeBandLookup { get; set; }

        public bool IsCoCriminalOffence { get; set; }
        public bool IsGenderBasedViolence { get; set; }

        public string? CaseInfoFolderPath { get; set; }
        public string? Remarks { get; set; }

        public string Status { get; set; } = "Active";

        public bool IsSuspended { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        [ForeignKey(nameof(CreatedBy))]
        public ApplicationUser? CreatedByUser { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        public ICollection<EvidenceDepositItem> Items { get; set; } = new List<EvidenceDepositItem>();
        public ICollection<EvidenceMoveHistory> MoveHistories { get; set; } = new List<EvidenceMoveHistory>();
    }
}