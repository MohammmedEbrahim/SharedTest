using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Accounts
{
    public partial class Account : IdentityUser
    {
		public string Name { get; set; }
        public string NickName { get; set; }
	    public byte[] MainImage { get; set; }
		public byte[] CoverImage { get; set; }
        public Role Role { get; set; }
    }
}
