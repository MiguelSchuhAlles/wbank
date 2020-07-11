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
using Banking.Shared.Responses;

namespace Banking.Application.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly IOperationService _operationService;

        public OperationsController(IOperationService operationService)
        {
            _operationService = operationService;
        }

        [HttpPost]
        [Route("deposit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Deposit([FromBody]OperationRequestDTO request)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var response = await _operationService.Deposit(request, userId);

            if (response.ResponseStatus != ResponseStatus.Success) return BadRequest(response);

            return Ok(response);
        }

        [HttpPost]
        [Route("ticketpayment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TicketPayment([FromBody]PaymentRequestDTO request)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var response = await _operationService.TicketPayment(request, userId);

            if (response.ResponseStatus != ResponseStatus.Success) return BadRequest(response);

            return Ok(response);
        }

        [HttpPost]
        [Route("withdraw")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> WithdrawAsync([FromBody]OperationRequestDTO request)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var response = await _operationService.Withdraw(request, userId);

            if (response.ResponseStatus != ResponseStatus.Success) return BadRequest(response);

            return Ok(response);
        }
    }
}
