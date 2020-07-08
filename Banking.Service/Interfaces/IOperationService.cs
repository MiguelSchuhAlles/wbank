﻿using Banking.Domain.Entities;
using Banking.Domain.Entities.Operations;
using Banking.Shared.Requests;
using Banking.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Banking.Service.Interfaces
{
    public interface IOperationService
    {
        Task<Response<Operation>> Deposit(OperationRequestDTO request, int userId);
        Task<Response<Operation>> TicketPayment(PaymentRequestDTO request, int userId);
        Task<Response<Operation>> Withdraw(OperationRequestDTO request, int userId);
    }
}
