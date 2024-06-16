using Application.Common;
using Application.Common.Interfaces;
using Application.Requests.Countries.Models;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Countries.Commands
{
	public class CountiresPutCommand : CountriesPostPutCommon, IRequest<CountryVm>
	{
		public int Id { get; set; }
		public class Validator : CommonValidator<CountiresPutCommand> { }

		public class Handler : IRequestHandler<CountiresPutCommand , CountryVm>
		{
			private readonly IApplicationDbContext _context;
			private readonly ICurrentUserService _currentUser;

			public Handler(IApplicationDbContext context , ICurrentUserService currentUser)
			{
				this._context = context;
				this._currentUser = currentUser;
			}

			public async Task<CountryVm> Handle(CountiresPutCommand request , CancellationToken cancellationToken)
			{
				var role = _currentUser.Get(UserClaims.Role);

				if (role != Role.Admin.ToString())
					throw new Exception("Access Denied");

				var country = await _context.Countries
					.FirstOrDefaultAsync(c => c.Id == request.Id , cancellationToken);

				if (country == null)
					throw new Exception("Country is Not Exists");

				country.Name = request.Name;
				country.Code = request.Code;

				await _context.SaveChangesAsync();

				return CountryVm.MapFrom(country);
			}
		}
	}
}
