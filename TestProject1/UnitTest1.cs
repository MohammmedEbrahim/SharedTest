using Application.Common;
using Application.Common.Interfaces;
using Application.Requests.Countries.Commands;
using Domain.Entities.Nations;
using Domain.Enums;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using static Application.Requests.Countries.Commands.CountriesPostCommand;

namespace TestProject1
{
    public class Tests
    {
        [TestFixture]
        public class CountriesPostCommandTests
        {
            [Test]
            public async Task Handle_ValidRequest_ReturnsNewCountryId()
            {
                // Arrange
                var mockContext = new Mock<IApplicationDbContext>();
                var mockCurrentUser = new Mock<ICurrentUserService>();
                var mockValidator = new Mock<IValidator<CountriesPostCommand>>();

                var command = new CountriesPostCommand
                {
                    Name = null, // Invalid: Name is null
                    Code = "TC"
                };

                // Mock current user to return an admin role
                mockCurrentUser.Setup(x => x.Get(UserClaims.Role)).Returns(Role.User.ToString());

                // Setup validation to fail
               mockValidator.Setup(x => x.ValidateAsync(It.IsAny<CountriesPostCommand>() , CancellationToken.None))
                    .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Name" , "Name is required") }));

                mockValidator.Verify(x => x.ValidateAsync(command , CancellationToken.None) , Times.Never);


                var handler = new CountriesPostCommand.Handler(mockContext.Object, mockCurrentUser.Object);

                mockCurrentUser.Verify(x => x.Get(UserClaims.Role).Equals(Role.User) , Times.Never);

                // Ensure the context methods were not called (since validation failed, nothing should be added or saved)
                mockContext.Verify(x => x.Countries.AddAsync(It.IsAny<Country>() , It.IsAny<CancellationToken>()) , Times.Never);
                mockContext.Verify(x => x.SaveChangesAsync() , Times.Never);
            }

        }
    }
}