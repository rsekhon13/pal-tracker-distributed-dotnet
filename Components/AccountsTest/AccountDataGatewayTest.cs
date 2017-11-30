using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Accounts;
using DatabaseSupport;
using TestSupport;
using Xunit;

namespace AccountsTest
{
    [Collection("Accounts")]
    public class AccountDataGatewayTest
    {
        private readonly DataSourceConfig _dataSourceConfig = new DataSourceConfig();
        private readonly TestDatabaseSupport _support = new TestDatabaseSupport();

        static AccountDataGatewayTest() => TestEnvSupport.SetRegistrationVcap();
        public AccountDataGatewayTest() => _support.TruncateAllTables();
        
        [Fact]
        public void TestCreate()
        {
            _support.ExecSql("insert into users (id, name) values (12, 'Jack');");

            var template = new DatabaseTemplate(_dataSourceConfig);
            var gateway = new AccountDataGateway(template);
            gateway.Create(12, "anAccount");

            var names = template.Query("select name from accounts", reader => reader.GetString(0),
                new List<DbParameter>());

            Assert.Equal("anAccount", names.First());
        }

        [Fact]
        public void TestFindBy()
        {
            _support.ExecSql(@"
insert into users (id, name) values (12, 'Jack');
insert into accounts (id, owner_id, name) values (1, 12, 'anAccount'), (2, 12, 'anotherAccount');
");

            var gateway = new AccountDataGateway(new DatabaseTemplate(_dataSourceConfig));

            var actual = gateway.FindBy(12);

            Assert.Equal(1, actual[0].Id);
            Assert.Equal(12, actual[0].OwnerId);
            Assert.Equal("anAccount", actual[0].Name);
            Assert.Equal(2, actual[1].Id);
            Assert.Equal(12, actual[1].OwnerId);
            Assert.Equal("anotherAccount", actual[1].Name);
        }
    }
}