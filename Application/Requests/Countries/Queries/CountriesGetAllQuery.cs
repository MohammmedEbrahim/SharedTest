using Application.Common;
using Application.Common.Interfaces;
using Application.Requests.Countries.Models;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Countries.Queries
{
	public class CountriesGetAllQuery : IRequest<List<CountryVm>>
	{
		public class Handler : IRequestHandler<CountriesGetAllQuery , List<CountryVm>>
		{
			private readonly IApplicationDbContext _context;
			private readonly ICurrentUserService _currentUser;

			public Handler(IApplicationDbContext context , ICurrentUserService currentUser)
			{
				this._context = context;
				this._currentUser = currentUser;
			}

			public async Task<List<CountryVm>> Handle(CountriesGetAllQuery request , CancellationToken cancellationToken)
			{
				var role = _currentUser.Get(UserClaims.Role);

				if (role != Role.Admin.ToString())
					throw new Exception("Access Denied");

				return await _context.Countries
					  .AsNoTracking()
					  .Select(c => CountryVm.MapFrom(c))
					  .ToListAsync(cancellationToken);
			}
		}
	}
}
