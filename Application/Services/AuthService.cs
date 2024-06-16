using Application.Common;
using Application.Common.Interfaces;
using Application.Helpers;
using Application.Requests.Accounts.Models;
using Domain.Entities.Accounts;
using Domain.Entities.Accounts.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Application.Services
{
	public class AuthService : IAuthService
	{
		private readonly IApplicationDbContext _context;
		private readonly JWT _jwt;

		public AuthService(IOptions<JWT> jwt , IApplicationDbContext context)
		{
			_context = context;
			_jwt     = jwt.Value;
		}

		public JwtSecurityToken GenerateToken(Account user)
		{
			var claims = new[]
			{
				//new Claim(JwtRegisteredClaimNames.Sub, user.UserName),               //sub means subject
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),   //Jti id for token 
				new Claim(UserClaims.Id, user.Id),
				new Claim(UserClaims.Role, user.Role.ToString()),
			};

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
			var signingCredentials   = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var jwtSecurityToken = new JwtSecurityToken(           // the values which he will use to create jwt 
				issuer: _jwt.Issuer,
				audience: _jwt.Audience,
				claims: claims,
				expires: DateTime.Now.AddDays(_jwt.TokenValidityInHours),
				signingCredentials: signingCredentials);

			return jwtSecurityToken;
		}

		public async Task<AccountVm> FindByUserName(string userName)
		{
			var account = await _context.Accounts
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.UserName == userName);

			if (account == null)
				throw new Exception("Account is Not Exist");

			return AccountVm.MapFrom(account);
		}

		public RefreshToken GenerateRefreshToken()
		{
			var randomNumber = new byte[64];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
			}

			using (var sha256 = SHA256.Create())
		    {
			    randomNumber = sha256.ComputeHash(randomNumber);
			}

			return new RefreshToken()
			{
				Token = Convert.ToBase64String(randomNumber) ,
				ExpiresOn = DateTime.Now.AddDays(5) ,
				CreatedOn = DateTime.Now
			};
		}
	}
}
