using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Domain.Entities.Operations
{
    public class Transference : BaseEntity
    {
        [Required]
        public TransferenceStatus Status { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int AccountId { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool SameTitularity { get; set; }

        [Required]
        public string RecipientName { get; set; }

        [Required]
        public string RecipientCode { get; set; }

        [Required]
        public string RecipientAccountCode { get; set; }

        [Required]
        public int RecipientBranchNumber { get; set; }

        [Required]
        public int RecipientBankNumber { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}
