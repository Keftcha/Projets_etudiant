using CK.Core;
using CK.SqlServer;
using CK.SqlServer.Setup;
using FluentAssertions;
using inProjects.Data;
using inProjects.Data.Queries;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static CK.Testing.DBSetupTestHelper;


namespace inProjects.Tests
{

    //[TestFixture]

    class TimePeriodTests
    {
        [Test]
        public async Task create_period()
        {
            DateTime dateTime = DateTime.Now;
            DateTime dateTime2 = DateTime.Now.AddDays(1);
            var timePeriod = TestHelper.StObjMap.StObjs.Obtain<TimePeriodTable>();
            using( var ctx = new SqlStandardCallContext(TestHelper.Monitor) )
            {
                var id = await timePeriod.CreateTimePeriodAsync( ctx, 1, dateTime, dateTime2, "I" );
                Assert.That( id > 0 );

            }

        }

        [Test]
        public async Task create_period_with_parent_zone()
        {
            DateTime dateTime = DateTime.Now;
            DateTime dateTime2 = DateTime.Now.AddDays( 1 );
            var timePeriod = TestHelper.StObjMap.StObjs.Obtain<TimePeriodTable>();
            using( var ctx = new SqlStandardCallContext( TestHelper.Monitor ) )
            {
                var id = await timePeriod.CreateTimePeriodAsync( ctx, 1, dateTime, dateTime2, "I", 4 );
                Assert.That( id > 0 );

            }

        }

        [Test]
        public async Task create_period_with_grant_access()
        {
            DateTime dateTime = DateTime.Now;
            DateTime dateTime2 = DateTime.Now.AddDays( 1 );
            var timePeriod = TestHelper.StObjMap.StObjs.Obtain<TimePeriodTable>();
            var sqldatabase = TestHelper.StObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            var userTable = TestHelper.StObjMap.StObjs.Obtain<CustomUserTable>();
            var group = TestHelper.StObjMap.StObjs.Obtain<CustomGroupTable>();

            bool result = false;
            using( var ctx = new SqlStandardCallContext( TestHelper.Monitor ) )
            {
                AclQueries aclQueries = new AclQueries( ctx, sqldatabase );
                var userId = userTable.CreateUser( ctx, 1, Guid.NewGuid().ToString(), "Admin", "Admin" );
               
                //17 is group Admin In intech
                await group.AddUserAsync( ctx, 1, 17, userId, true );
                //Acl ID 9 is AclManagementId of Zone ID InTech
                if(await aclQueries.VerifyGrantLevelByUserId(112,9,userId,Operator.SuperiorOrEqual) == true )
                {
                    var id = await timePeriod.CreateTimePeriodAsync( ctx, 1, dateTime, dateTime2, "I", 4 );
                    result = true;
                    Assert.That( id > 0 );
                }

                Assert.That( result == true );
            }

        }


        [Test]
        public async Task deny_create_period()
        {
            DateTime dateTime = DateTime.Now;
            DateTime dateTime2 = DateTime.Now.AddDays( 1 );
            var timePeriod = TestHelper.StObjMap.StObjs.Obtain<TimePeriodTable>();
            var sqldatabase = TestHelper.StObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            var userTable = TestHelper.StObjMap.StObjs.Obtain<CustomUserTable>();
            var group = TestHelper.StObjMap.StObjs.Obtain<CustomGroupTable>();

            bool result = false;
            using( var ctx = new SqlStandardCallContext( TestHelper.Monitor ) )
            {
                AclQueries aclQueries = new AclQueries( ctx, sqldatabase );
                var userId = userTable.CreateUser( ctx, 1, Guid.NewGuid().ToString(), "NotAdmin", "NotAdmin" );
                //Acl ID 9 is AclManagementId of Zone ID InTech
                if( await aclQueries.VerifyGrantLevelByUserId( 127, 9, userId, Operator.SuperiorOrEqual ) == true )
                {
                    var id = await timePeriod.CreateTimePeriodAsync( ctx, 1, dateTime, dateTime2, "I", 4 );
                    result = true;
                    Assert.That( id > 0 );
                }

                Assert.That( result == false );
            }

        }


    }
}
