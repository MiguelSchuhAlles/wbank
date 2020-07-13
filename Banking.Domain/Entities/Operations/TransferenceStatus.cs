using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Domain.Entities.Operations
{
    public enum TransferenceStatus
    {
        Pending = 0, //Debit, cash purchases
        Processed = 1,
        Rejected = 2
    }
}
