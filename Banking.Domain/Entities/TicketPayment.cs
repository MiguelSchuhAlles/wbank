using Banking.Domain.Entities.Operations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Domain.Entities
{
    public class TicketPayment : BaseEntity
    {
        [Required]
        public int OperationId { get; set; }

        [Required, MaxLength(50)]
        public string Code { get; set; }

        [ForeignKey("OperationId")]
        public virtual Operation Operation { get; set; }
    }
}
