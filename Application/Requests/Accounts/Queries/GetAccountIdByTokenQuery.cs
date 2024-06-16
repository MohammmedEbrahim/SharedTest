using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests.Accounts.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Requests.Accounts.Queries
{
	public class GetAccountIdByTokenQuery : IRequest<AccountVm>
	{
		public class Handler : IRequestHandler<GetAccountIdByTokenQuery, AccountVm>
		{
			private readonly IHttpContextAccessor _httpContextAccessor;

			public Handler(IHttpContextAccessor httpContextAccessor)
			{
				_httpContextAccessor = httpContextAccessor;
			}
			public async Task<AccountVm> Handle(GetAccountIdByTokenQuery request, CancellationToken cancellationToken)
			{
				return new AccountVm()
				{
					Email = _httpContextAccessor.HttpContext.User
						.FindFirstValue(ClaimTypes.Sid) ,
					
					UserName  = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId")
				};
			}
		}
	}
}
