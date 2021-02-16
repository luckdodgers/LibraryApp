using NUnit.Framework;
using System.Threading.Tasks;

namespace LibraryApp.Tests.Application.IntegrationTests
{
    using static TestSetup;
    class BaseTest
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await ResetState();
        }
    }
}
