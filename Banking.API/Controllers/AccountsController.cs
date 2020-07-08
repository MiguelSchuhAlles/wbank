using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Security.Claims;
using Banking.Shared.Requests;
using Banking.Domain.Entities.Operations;

namespace Banking.Application.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async ValueTask<IActionResult> GetById(int id)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            return Ok(await _accountService.GetAccount(id, userId));
        }

        [HttpGet, Route("{id}/balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async ValueTask<IActionResult> GetBalance(int id)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            return Ok(await _accountService.GetBalance(id, userId));
        }

        [HttpGet, Route("{id}/history")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async ValueTask<IActionResult> GetTransactionHistory(int id, DateTime start)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            return Ok(await _accountService.GetOperationHistory(id, userId, start));
        }

        [HttpGet, Route("{id}/debits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async ValueTask<IActionResult> GetDebitsHistory(int id, DateTime start)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var debitOps = new OperationType[]
            {
                OperationType.PointOfSale,
                OperationType.Transfer,
                OperationType.Withdrawal,
                OperationType.Charge,
                OperationType.Payment
            };

            return Ok(await _accountService.GetOperationHistory(id, userId, start, operation => debitOps.Contains(operation.OperationType)));
        }

        [HttpGet, Route("{id}/credits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async ValueTask<IActionResult> GetCreditsHistory(int id, DateTime start)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var creditOps = new OperationType[]
            {
                OperationType.Deposit
            };

            return Ok(await _accountService.GetOperationHistory(id, userId, start, operation => creditOps.Contains(operation.OperationType)));
        }
    }
}
