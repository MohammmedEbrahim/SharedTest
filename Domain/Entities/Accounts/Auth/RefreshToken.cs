using System;

namespace Domain.Entities.Accounts.Auth
{
	public partial class RefreshToken
	{
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsExpires => DateTime.Now >= ExpiresOn;
    }

	public partial class RefreshToken
    {
        public string AccountId { get; set; }
        public Account Account { get; set; }
    }
}
