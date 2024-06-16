using Application.Requests.Countries.Commands;
using Domain.Entities.Nations;

namespace XUnit_Project
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var command = new CountriesPostCommand
            {
                Name = "co" ,
                Code = "nh"
            };

            var id = await SendAsync(command);

            var cq = await FindAsync<Country>(id);

            cq.Title.Should().Be(command.Name);
            cq.Answer.Should().Be(command.Code);
        }
    }
}