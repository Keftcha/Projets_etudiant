using CK.SqlServer;
using inProjects.Data;
using NUnit.Framework;
using CK.Core;
using CK.DB.Actor;
using System.Threading.Tasks;
using static CK.Testing.DBSetupTestHelper;
using inProjects.Data.Queries;
using CK.SqlServer.Setup;
using inProjects.Data.Data.ProjectStudent;
using System;

namespace inProjects.Tests
{
    [TestFixture]
    class ProjectStudentTests
    {
        [Test]
        public async Task create_a_project()
        {
            var projectStudent = TestHelper.StObjMap.StObjs.Obtain<ProjectStudentTable>();

            using( var ctx = new SqlStandardCallContext() )
            {
                var ProjectCreate = await projectStudent.CreateProjectStudent( ctx, 1, 2, "name", 1, "a;b;c", "aaa", "okok", "wwww", 1, "I" );
                Assert.That( ProjectCreate.ProjectStudentId > 0 );
            }
        }

        [Test]

        public async Task add_url_to_a_project()
        {
            var urlTable = TestHelper.StObjMap.StObjs.Obtain<ProjectUrlTable>();
            var projectStudent = TestHelper.StObjMap.StObjs.Obtain<ProjectStudentTable>();
            var sqlDatabase = TestHelper.StObjMap.StObjs.Obtain<SqlDefaultDatabase>();

            using( var ctx = new SqlStandardCallContext() )
            {
                ProjectQueries projectQueries = new ProjectQueries( ctx, sqlDatabase );

                var ProjectCreate = await projectStudent.CreateProjectStudent( ctx, 1, 2, "name", 1, "a;b;c", "aaa", "okok", "wwww", 1, "I" );

                var ProjectUrlCreate = await urlTable.CreateOrUpdateProjectUrl( ctx, ProjectCreate.ProjectStudentId, "www.aaa.fr", "Github" );
                ProjectUrlData urlData = await projectQueries.GetUrlByProject( ProjectCreate.ProjectStudentId );

                Assert.That( urlData.Url == "www.aaa.fr" );
            }
        }

        [Test]

        public async Task update_url_of_a_project()
        {
            var urlTable = TestHelper.StObjMap.StObjs.Obtain<ProjectUrlTable>();
            var projectStudent = TestHelper.StObjMap.StObjs.Obtain<ProjectStudentTable>();
            var sqlDatabase = TestHelper.StObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            using( var ctx = new SqlStandardCallContext() )
            {
                ProjectQueries projectQueries = new ProjectQueries( ctx, sqlDatabase );

                var ProjectCreate = await projectStudent.CreateProjectStudent( ctx, 1, 2, "name", 1, "a;b;c", "aaa", "okok", "wwww", 1, "I" );
                var ProjectUrlCreate = await urlTable.CreateOrUpdateProjectUrl( ctx, ProjectCreate.ProjectStudentId, "www.aaa.fr", "Github" );
                var ProjectUrlUpdate = await urlTable.CreateOrUpdateProjectUrl( ctx, ProjectCreate.ProjectStudentId, "www.bbb.fr", "Github" );
                ProjectUrlData urlData = await projectQueries.GetUrlByProject( ProjectCreate.ProjectStudentId );

                Assert.That( urlData.Url == "www.bbb.fr" );


            }

        }

        [Test]

        public async Task a_user_fav_a_project()
        {
            var projectStudent = TestHelper.StObjMap.StObjs.Obtain<ProjectStudentTable>();
            var userTable = TestHelper.StObjMap.StObjs.Obtain<UserTable>();
            var userFavTable = TestHelper.StObjMap.StObjs.Obtain<UserFavProjectTable>();
            var sqlDatabase = TestHelper.StObjMap.StObjs.Obtain<SqlDefaultDatabase>();

            using( var ctx = new SqlStandardCallContext() )
            {
                ProjectQueries projectQueries = new ProjectQueries( ctx, sqlDatabase );
                var ProjectCreate = await projectStudent.CreateProjectStudent( ctx, 1, 2, "name", 1, "a;b;c", "aaa", "okok", "wwww", 1, "I" );
                var userCreate = await userTable.CreateUserAsync( ctx, 1, Guid.NewGuid().ToString());
                await userFavTable.FavOrUnfavProject(ctx, userCreate, ProjectCreate.ProjectStudentId);

                //UserFavProjectData userFavProjectData = await projectQueries.GetSPecificFavOfUser( userCreate, ProjectCreate.ProjectStudentId );
               // Assert.That( userFavProjectData != null );


            }
        }


        [Test]

        public async Task a_user_Unfav_a_project()
        {
            var projectStudent = TestHelper.StObjMap.StObjs.Obtain<ProjectStudentTable>();
            var userTable = TestHelper.StObjMap.StObjs.Obtain<UserTable>();
            var userFavTable = TestHelper.StObjMap.StObjs.Obtain<UserFavProjectTable>();
            var sqlDatabase = TestHelper.StObjMap.StObjs.Obtain<SqlDefaultDatabase>();
           
            using( var ctx = new SqlStandardCallContext() )
            {
                ProjectQueries projectQueries = new ProjectQueries( ctx, sqlDatabase );
                var ProjectCreate = await projectStudent.CreateProjectStudent( ctx, 1, 2, "name", 1, "a;b;c", "aaa", "okok", "wwww", 1, "I" );
                var userCreate = await userTable.CreateUserAsync( ctx, 1, Guid.NewGuid().ToString() );
                await userFavTable.FavOrUnfavProject( ctx, userCreate, ProjectCreate.ProjectStudentId );
                await userFavTable.FavOrUnfavProject( ctx, userCreate, ProjectCreate.ProjectStudentId );

                //UserFavProjectData userFavProjectData = await projectQueries.GetSPecificFavOfUser( userCreate, ProjectCreate.ProjectStudentId );
               // Assert.That( userFavProjectData == null );


            }
        }
    }
}
