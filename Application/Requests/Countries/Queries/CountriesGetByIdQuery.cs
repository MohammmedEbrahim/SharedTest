using Application.Common;
using Application.Common.Interfaces;
using Application.Requests.Countries.Models;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Countries.Queries
{
	public class CountriesGetByIdQuery : IRequest<CountryVm>
	{
		public int Id { get; set; }
		public class Handler : IRequestHandler<CountriesGetByIdQuery , CountryVm>
		{
			private readonly IApplicationDbContext _context;
			private readonly ICurrentUserService _currentUser;

			public Handler(IApplicationDbContext context , ICurrentUserService currentUser)
			{
				this._context = context;
				this._currentUser = currentUser;
			}

			public async Task<CountryVm> Handle(CountriesGetByIdQuery request , CancellationToken cancellationToken)
			{
				var role = _currentUser.Get(UserClaims.Role);

				if (role != Role.Admin.ToString())
					throw new Exception("Access Denied");

				var country = await _context.Countries
					.AsNoTracking()
					.FirstOrDefaultAsync(c => c.Id == request.Id ,cancellationToken);

				if (country == null)
					throw new Exception("Country is Not Exists");

				return CountryVm.MapFrom(country);
			}
		}
	}
}
