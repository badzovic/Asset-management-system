using Microsoft.AspNetCore.Identity;
using AMS_data.Entities.Lookups;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMS_data.Entities.Narcotics
{
    public class NarcoticsDeposit
    {
        public int Id { get; set; }

        public string RegistrationNo { get; set; } = string.Empty;

        public string? CaseNo { get; set; }

        public int? CaseTypeLookupId { get; set; }
        public LookupItem? CaseTypeLookup { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public int? OUPerformedSeizureLookupId { get; set; }
        public LookupItem? OUPerformedSeizureLookup { get; set; }

        public string? StorageOrderNo { get; set; }
        public DateTime? StorageOrderDate { get; set; }

        public string? LinkToOrderNo { get; set; }
        public string? ConfirmStorageOrderNo { get; set; }

        public int? DepositLocationLookupId { get; set; }
        public LookupItem? DepositLocationLookup { get; set; }

        public int? EvidenceIndicatorLookupId { get; set; }
        public LookupItem? EvidenceIndicatorLookup { get; set; }

        public string? SubmittedByOfficer { get; set; }
        public string? HandlingOfficer { get; set; }

        public string? ForensicReportNo { get; set; }
        public DateTime? ForensicReportDate { get; set; }

        public string? VerdictNo { get; set; }
        public DateTime? VerdictDate { get; set; }

        public string? DestructionOrderNo { get; set; }
        public DateTime? DestructionOrderDate { get; set; }

        public DateTime? DestructionDate { get; set; }

        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? Address { get; set; }
        public string? PersonalIdNo { get; set; }

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

        [ForeignKey(nameof(UpdatedBy))]
        public ApplicationUser? UpdatedByUser { get; set; }

        public ICollection<NarcoticsDepositItem> Items { get; set; } = new List<NarcoticsDepositItem>();
        public ICollection<NarcoticsMoveHistory> MoveHistories { get; set; } = new List<NarcoticsMoveHistory>();
    }
}