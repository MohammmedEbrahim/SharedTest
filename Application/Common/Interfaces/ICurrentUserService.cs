namespace Application.Common.Interfaces
{
	public interface ICurrentUserService
	{
		public string Get(string claim);
	}
}
