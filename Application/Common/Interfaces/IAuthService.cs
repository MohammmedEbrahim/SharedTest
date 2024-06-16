using Application.Requests.Accounts.Models;
using Domain.Entities.Accounts;
using Domain.Entities.Accounts.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
	public interface IAuthService
	{
		public JwtSecurityToken GenerateToken(Account user);

		public Task<AccountVm> FindByUserName(string userName);

		public RefreshToken GenerateRefreshToken();
	}
}
