using Application.Requests.Accounts.Commands;
using Application.Requests.Accounts.Models;
using Application.Requests.Accounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SharedZone.Server.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class AccountController : ControllerBase
	{
		private readonly IMediator _mediator;

		public AccountController(IMediator mediator) => this._mediator = mediator;

		[Authorize]
		[HttpGet]
		public async Task<ActionResult<List<AccountVm>>> GetAll() => await _mediator.Send(new AccountsGetListQuery());
		
		[HttpPost]
		public async Task<ActionResult<TokenResponse>> Register([FromBody] AccountsRegisterCommand command) => await _mediator.Send(command);

		[HttpPost("[action]")]
		public async Task<ActionResult<TokenResponse>> LogIn([FromBody] AccountsLoginCommand command) => await _mediator.Send(command);
	}
}
