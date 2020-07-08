using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public int Number { get; set; }
    }
}
