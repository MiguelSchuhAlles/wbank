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

namespace Banking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferenceController : ControllerBase
    {
        private readonly ITransferenceService _transferenceService;

        public TransferenceController(ITransferenceService transferenceService)
        {
            _transferenceService = transferenceService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Transference([FromBody]TransferenceRequestDTO request)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var response = await _transferenceService.Transference(request, userId);

            if (response.ResponseStatus != ResponseStatus.Success) return BadRequest(response);

            return Ok(response);
        }
    }
}