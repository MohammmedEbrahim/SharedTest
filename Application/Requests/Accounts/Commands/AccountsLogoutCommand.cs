using Domain.Entities.Accounts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Accounts.Commands
{
	public class AccountsLogoutCommand : IRequest<string>
	{
		public class Handler : IRequestHandler<AccountsLogoutCommand , string>
		{
			private readonly UserManager<Account> _userManager;
			private readonly SignInManager<Account> _signInManager;
			private readonly IHttpContextAccessor _httpContextAccessor;

			public Handler(UserManager<Account> userManager
				, SignInManager<Account> signInManager
				, IHttpContextAccessor httpContextAccessor)
			{
				_userManager         = userManager;
				_signInManager       = signInManager;
				_httpContextAccessor = httpContextAccessor;
			}

			public async Task<string> Handle(AccountsLogoutCommand request , CancellationToken cancellationToken)
			{
				var accountEmail = _httpContextAccessor.HttpContext.User
					.FindFirstValue(ClaimTypes.Email);

				var account = await _userManager.FindByEmailAsync(accountEmail);

				if ( account == null )
					throw new Exception("Invalid Data");

				await _signInManager.SignOutAsync();

				return "SigOut is Done";
			}
		}
	}
}
