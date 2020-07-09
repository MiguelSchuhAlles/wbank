using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Domain.Entities.Operations
{
    public enum OperationType
    {
        PointOfSale = 0, //Debit, cash purchases
        Transfer = 1,
        Withdrawal = 2,
        Deposit = 3,
        Charge = 4, //credit card
        Payment = 5,
        InterestIncome = 6
    }
}
