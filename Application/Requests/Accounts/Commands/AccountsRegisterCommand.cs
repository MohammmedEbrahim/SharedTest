using Application.Common.Interfaces;
using Application.Requests.Accounts.Models;
using Domain.Entities.Accounts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Requests.Accounts.Commands
{
	public class AccountsRegisterCommand : AccountsPostPutCommon  , IRequest<TokenResponse> //todo : just register only teacher & student not admin
	{
		public class Validator : CommonValidator<AccountsRegisterCommand>
		{
			public Validator(IApplicationDbContext context) : base(context) { }
		}
		
		public class Handler : IRequestHandler<AccountsRegisterCommand, TokenResponse>
		{
			private readonly UserManager<Account> _userManager;
			private readonly IAuthService _authService;
			private readonly IApplicationDbContext _context;

			public Handler(UserManager<Account> userManager
				         , IAuthService authService
				         , IApplicationDbContext context)
			{
				_userManager  = userManager;
				_authService  = authService;
				_context      = context;
			}
			
			public async Task<TokenResponse> Handle(AccountsRegisterCommand request, CancellationToken cancellationToken)
			{
				var isNameDuplicated = await _context.Accounts
					.AnyAsync(x => x.UserName == request.UserName, cancellationToken: cancellationToken);

				if (isNameDuplicated)
					throw new Exception("UserName is already Exists");

				var isEmailDuplicated = await _context.Accounts
					.AnyAsync(x => x.Email == request.Email, cancellationToken: cancellationToken);

				if (isEmailDuplicated)
					throw new Exception("Email is already Exists");

				var account = new Account()
				{
					Email         = request.Email,
					UserName      = request.UserName,
					PhoneNumber   = request.PhoneNumber,
					Role          = request.Role,
				};
				
				var result = await _userManager.CreateAsync(account, request.Password);

				if (!result.Succeeded)
					throw new Exception("Failed to create new Account");
				
				var jwtSecurityToken = _authService.GenerateToken(account);

				return new TokenResponse()
				{
					Token        = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken) ,
				};
			}
		}
	}
}
