using Application.Common;
using Application.Common.Interfaces;
using Domain.Entities.Nations;
using Domain.Enums;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Countries.Commands
{
	public class CountriesPostCommand : CountriesPostPutCommon, IRequest<int>
	{
		public class Validator : CommonValidator<CountriesPostCommand> { }

		public class Handler : IRequestHandler<CountriesPostCommand , int>
		{
			private readonly IApplicationDbContext _context;
			private readonly ICurrentUserService _currentUser;

			public Handler(IApplicationDbContext context , ICurrentUserService currentUser)
			{
				this._context     = context;
				this._currentUser = currentUser;
			}

			public async Task<int> Handle(CountriesPostCommand request , CancellationToken ct)
			{
				var role = _currentUser.Get(UserClaims.Role);

				if (role != Role.Admin.ToString())
				   throw new Exception("Access Denied");

				var country = new Country
				{
					Name = request.Name,
					Code = request.Code // need to add order
				};

				// validate that country with name or code can't be duplicated

				await _context.Countries.AddAsync(country , ct);
				await _context.SaveChangesAsync();

				return country.Id;
			}
		}
	}
}
