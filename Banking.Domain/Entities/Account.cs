using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Domain.Entities
{
    public class Account : BaseEntity
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Required]
        public bool Enabled { get; set; } = true;

        [Required]
        public decimal Balance { get; set; } = 0;

        [Required]
        public bool Rentabilize { get; set; } = true;

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
