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

namespace Banking.Application.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async ValueTask<IActionResult> GetAll(CancellationToken ct = default)
        {
            var users = await _userService.GetAll(ct);
            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async ValueTask<IActionResult> GetById(int id, CancellationToken ct = default)
        {
            var user = await _userService.GetById(id, ct);
            return Ok(user);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async ValueTask<IActionResult> Post([FromBody] UserRequestDTO user)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async ValueTask<IActionResult> Put(int id, [FromBody] UserRequestDTO user)
        {
            throw new NotImplementedException();
        }

        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async ValueTask<IActionResult> GetByToken(CancellationToken ct = default)
        {
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var user = await _userService.GetById(userId, ct);
            return Ok(user);
        }
    }
}
