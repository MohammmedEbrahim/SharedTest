//using Application.Common.Interfaces;
//using Application.Requests.Accounts.Models;
//using MediatR;
//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Application.Requests.Accounts.Queries
//{
//	public class AccountsGetByUserNameQuery : IRequest<AccountVm>
//	{
//		public string UserName { get; set; }

//		public class Handler : IRequestHandler<AccountsGetByUserNameQuery, AccountVm>
//		{
//			private readonly IAuthService _authService;

//			public Handler(IAuthService authService)
//			{
//				_authService = authService;
//			}

//			public async Task<AccountVm> Handle(AccountsGetByUserNameQuery request, CancellationToken cancellationToken)
//			{
//				if(request.UserName == null) 
//					throw new ArgumentNullException(nameof(request.UserName));

//				//return await _authService.FindByUserName(request.UserName);
//				return new AccountVm();
//			}
//		}
//	}
//}
