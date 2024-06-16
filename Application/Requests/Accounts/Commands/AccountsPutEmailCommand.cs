using Application.Common.Interfaces;
using Application.Requests.Accounts.Models;
using Domain.Entities.Accounts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Accounts.Commands
{
	public class AccountsPutEmailCommand : IRequest<AccountVm>
	{
		public string Password { get; set; }
		public string Email { get; set; }
		public class Validator : AbstractValidator<AccountsPutEmailCommand>
		{
			public Validator()
			{
				RuleFor(x => x.Email)
					.NotNull()
					.WithMessage("Email can't be null");

				RuleFor(a => a.Password)
					.NotEmpty()
					.MinimumLength(8)
					.Matches("[A-Z]").WithMessage(" Password must contain one or more capital letters.")
					.Matches("[a-z]").WithMessage(" Password must contain one or more lowercase letters.")
					.Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage(" Password must contain one or more special characters.")
					.WithMessage(" Password contains that is not allowed.");
			}

			public class Handler : IRequestHandler<AccountsPutEmailCommand, AccountVm>
			{
				private readonly IApplicationDbContext _context;
				private readonly IHttpContextAccessor _httpContextAccessor;
				private readonly UserManager<Account> _userManager;

				public Handler(IApplicationDbContext context
				, IHttpContextAccessor httpContextAccessor
				,UserManager<Account> userManager)
				{
					_context             = context;
					_httpContextAccessor = httpContextAccessor;
					_userManager         = userManager;
				}

				public async Task<AccountVm> Handle(AccountsPutEmailCommand request, CancellationToken cancellationToken)
				{
					var accountId = _httpContextAccessor.HttpContext
						.User.FindFirstValue("userId");

					var account = await _context.Accounts
						.FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);

					var result = await _userManager.CheckPasswordAsync(account, request.Password);

					if ( !result )
						throw new Exception("Password doesn't match the current");

					account.Email = request.Email;

					await _context.SaveChangesAsync();

					return AccountVm.MapFrom(account);
				}
			}
		}
	}
}
