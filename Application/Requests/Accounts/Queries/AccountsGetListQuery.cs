using Application.Common.Interfaces;
using Application.Requests.Accounts.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Accounts.Queries
{
	public class AccountsGetListQuery : IRequest<List<AccountVm>>
	{
		public class Handler : IRequestHandler<AccountsGetListQuery , List<AccountVm>>
		{
			private readonly IApplicationDbContext _context;

			public Handler(IApplicationDbContext context)
			{
				_context = context;
			}

			public async Task<List<AccountVm>> Handle(AccountsGetListQuery request , CancellationToken cancellationToken)
				=> await _context.Accounts
					.AsNoTracking()
					.Select(a => AccountVm.MapFrom(a))
					.ToListAsync(cancellationToken);
		}
	}
}
