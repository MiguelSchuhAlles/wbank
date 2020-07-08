using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Domain.Entities.Operations
{
    public class Operation : BaseEntity
    {
        [Required]
        public OperationType OperationType { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int AccountId { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public virtual decimal Balance { get; set; } //after operation

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}
