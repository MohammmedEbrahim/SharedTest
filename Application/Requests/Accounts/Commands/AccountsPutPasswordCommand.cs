using Application.Common.Interfaces;
using Domain.Entities.Accounts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Accounts.Commands
{
	public class AccountsPutPasswordCommand : IRequest<string>
	{
		public string Id { get; set; }
		public string CurrentPassword { get; set; }
		public string NewPassword { get; set; }

		public class Validator : AbstractValidator<AccountsPutPasswordCommand>
		{
			public Validator()
			{
				RuleFor(a => a.CurrentPassword)
					.NotEmpty()
					.MinimumLength(8)
					.Matches("[A-Z]").WithMessage(" Password must contain one or more capital letters.")
					.Matches("[a-z]").WithMessage(" Password must contain one or more lowercase letters.")
					.Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage(" Password must contain one or more special characters.")
					.WithMessage(" Current Password contains that is not allowed.");

				RuleFor(a => a.NewPassword)
					.NotEmpty()
					.MinimumLength(8)
					.Matches("[A-Z]").WithMessage(" Password must contain one or more capital letters.")
					.Matches("[a-z]").WithMessage(" Password must contain one or more lowercase letters.")
					.Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage(" Password must contain one or more special characters.")
					.WithMessage(" New Password contains that is not allowed.");
			}
		}

		public class Handler : IRequestHandler<AccountsPutPasswordCommand, string>
		{
			private readonly IApplicationDbContext _context;
			private readonly IHttpContextAccessor _httpContextAccessor;
			private readonly UserManager<Account> _userManager;

			public Handler(IApplicationDbContext context 
				, IHttpContextAccessor httpContextAccessor
				, UserManager<Account> userManager)
			{
				_context             = context;
				_httpContextAccessor = httpContextAccessor;
				_userManager         = userManager;
			}

			public async Task<string> Handle(AccountsPutPasswordCommand request, CancellationToken cancellationToken)
			{
				var accountId = _httpContextAccessor.HttpContext
					.User.FindFirstValue("userId");

				if ( request.Id != accountId )
					throw new Exception("Access Denied");
				
				var account = await _userManager.FindByIdAsync(accountId);
				
				var result = await _userManager.CheckPasswordAsync(account, request.CurrentPassword);

				if (!result)
					throw new Exception("Password doesn't match the current");

				var isUpdated = await _userManager.ChangePasswordAsync(account, request.CurrentPassword, request.NewPassword);

				if (!isUpdated.Succeeded)
					return "Password Changed Failed";
				
				return "Password Changed Successfully";
			}
		}
	}
}
