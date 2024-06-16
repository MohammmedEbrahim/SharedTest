using Application.Requests.Countries.Commands;
using Application.Requests.Countries.Models;
using Application.Requests.Countries.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SharedZone.Server.Controllers
{
    [Authorize]
    [ApiController]
	[Route("api/[controller]")]
	public class CountryController : Controller
	{
		private readonly IMediator _mediator;

		public CountryController(IMediator mediator) => this._mediator = mediator;

		[HttpGet]
		public async Task<ActionResult<List<CountryVm>>> GetCountries() => await _mediator.Send(new CountriesGetAllQuery());

		[HttpGet("{id}")]
		public async Task<ActionResult<CountryVm>> Get(int id) => await _mediator.Send(new CountriesGetByIdQuery { Id = id });

		[HttpPost]
		public async Task<ActionResult<int>> Post([FromBody] CountriesPostCommand command) => await _mediator.Send(command);

		[HttpPut]
		public async Task<ActionResult<CountryVm>> Put([FromBody] CountiresPutCommand command) => await _mediator.Send(command);

		[HttpDelete("{id}")]
		public async Task<ActionResult<CountryVm>> Delete(int id) => await _mediator.Send(new CountriesDeleteCommand { Id = id });
	}
}
