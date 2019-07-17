using CK.Core;
using CK.DB.Auth;
using CK.DB.HZone;
using CK.DB.User.UserPassword;
using CK.SqlServer;
using inProjects.Data;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentAssertions;
using static CK.Testing.DBSetupTestHelper;
using CK.DB.Actor;
using System;

namespace inProjects.Tests
{
    [TestFixture]
    public class DBSetup : CK.DB.Tests.DBSetup
    {
        [Test]
        [Explicit]
        public void Add_Admin()
        {
            var u = TestHelper.StObjMap.StObjs.Obtain<UserPasswordTable>();

            using( var ctx = new SqlStandardCallContext( TestHelper.Monitor ) )
            {
                var result = u.CreateOrUpdatePasswordUser( ctx, 1, 1, "a" );

                Assert.That( result.OperationResult == UCResult.Created || result.OperationResult == UCResult.Updated );
            }
        }
        [Test]
        [Explicit]
        public async Task Add_School()
        {
            var s = TestHelper.StObjMap.StObjs.Obtain<SchoolTable>();
            using( var ctx = new SqlStandardCallContext( TestHelper.Monitor ) )
            {
                var result = await s.CreateSchool( ctx, 1, Guid.NewGuid().ToString() );
                Assert.That( result > 2 );
            }
        }

    }
}
