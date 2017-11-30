using Accounts;
using DatabaseSupport;
using TestSupport;
using Users;
using Xunit;

namespace AccountsTest
{
    [Collection("Accounts")]
    public class RegistrationServiceTest
    {
        private readonly DataSourceConfig _dataSourceConfig = new DataSourceConfig();
        private readonly TestDatabaseSupport _support = new TestDatabaseSupport();

        static RegistrationServiceTest() => TestEnvSupport.SetRegistrationVcap();
        public RegistrationServiceTest() => _support.TruncateAllTables();

        [Fact]
        public void TestFindBy()
        {
            var userDataGateway = new UserDataGateway(new DatabaseTemplate(_dataSourceConfig));
            var accountDataGateway = new AccountDataGateway(new DatabaseTemplate(_dataSourceConfig));
            var service = new RegistrationService(userDataGateway, accountDataGateway);

            var info = service.CreateUserWithAccount("aUser");

            Assert.Equal("aUser", info.Name);
        }
    }
}