using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
	public interface IBanService
	{
		public Task<bool> IsBanned (string firstAccountId, string secondAccountId);
	}
}
