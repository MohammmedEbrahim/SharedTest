using Application.Common.Interfaces;
using Application.Requests.Accounts.Models;
using Domain.Entities.Accounts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Accounts.Commands
{
    public class AccountsLoginCommand : IRequest<TokenResponse>
	{
		public string Email { get; set; }
		public string Password { get; set; }
		
		public class Validator : AbstractValidator<AccountsLoginCommand>
		{
			public Validator()
			{
				RuleFor(l => l.Email)
					.NotEmpty()
					.WithMessage("Email Can't be null");
				
				RuleFor(l => l.Password)
					.NotEmpty()
					.WithMessage("Password Can't be null");
			}
		}

		public class Handler : IRequestHandler<AccountsLoginCommand , TokenResponse>
		{
			private readonly UserManager<Account> _userManager;
			private readonly IAuthService _authService;
			private readonly IApplicationDbContext _context;

			public Handler(UserManager<Account> userManager
						 , IAuthService authService
				         , IApplicationDbContext context)
			{
				_userManager   = userManager;
				_authService   = authService;
				_context       = context;
			}

			public async Task<TokenResponse> Handle(AccountsLoginCommand request , CancellationToken cancellationToken)
			{
				var account = await _userManager.FindByEmailAsync(request.Email);

				if ( account == null )
					throw new Exception("Invalid Login Data");

				var IsValidPassword = await _userManager
					.CheckPasswordAsync(account , request.Password);

				if ( !IsValidPassword)
					throw new Exception("Invalid Login Data");

				var jwtSecurityToken =  _authService.GenerateToken(account);

				return new TokenResponse()
				{
					Token        = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken) ,
				};
			}
		}
	}
}
