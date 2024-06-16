using NUnit.Framework;
using System.Threading.Tasks;

namespace XUnit_Project
{
    using static Testing;

    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp() => await ResetState();
    }
}