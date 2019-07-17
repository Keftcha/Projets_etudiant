
using static CK.Testing.DBSetupTestHelper;
using CK.Core;

using inProjects.Data;
using NUnit.Framework;
using System.Threading.Tasks;
using CK.SqlServer;
using System;
using inProjects.Data.Queries;
using CK.SqlServer.Setup;
using CK.DB.Zone;
using inProjects.Data.Data.TimedUser;
using System.Collections.Generic;

namespace inProjects.Tests
{
    [TestFixture]
    class InsertTests
    {
        [Test]

        public async Task insert_user()
        {
            var userTable = TestHelper.StObjMap.StObjs.Obtain<CustomUserTable>();
            var userTimed = TestHelper.StObjMap.StObjs.Obtain<TimedUserTable>();
            var school = TestHelper.StObjMap.StObjs.Obtain<SchoolTable>();
            var group = TestHelper.StObjMap.StObjs.Obtain<GroupTable>();
            var projectStudent = TestHelper.StObjMap.StObjs.Obtain<ProjectStudentTable>();

            var timePeriod = TestHelper.StObjMap.StObjs.Obtain<TimePeriodTable>();
            var sqlDatabase = TestHelper.StObjMap.StObjs.Obtain<SqlDefaultDatabase>();

            DateTime dateTime = DateTime.Now;
            DateTime dateTime2 = DateTime.Now.AddDays( 1 );
            
            using( var ctx = new SqlStandardCallContext() )
            {
                TimedUserQueries timedUserQueries = new TimedUserQueries( ctx, sqlDatabase );
                //3 is the id of Intech
                var id = await timePeriod.CreateTimePeriodAsync( ctx, 1, dateTime, dateTime2, "S", 3 );
                List<TimedUserStruct> timedStudents = new List<TimedUserStruct>();
                List<TimedUserStruct> timedStaffMembers = new List<TimedUserStruct>();
                List<TimedUserStruct> timedJurys = new List<TimedUserStruct>();
                await create_zone();

                for( int i = 0; i < 100; i++ )
                {

                    int userType;

                    string userName = Guid.NewGuid().ToString();
                    string firstName = "Prenom: " + Guid.NewGuid();
                    string lastName = "Nom de famille: " + Guid.NewGuid();
                    string logo = "Logo: " + Guid.NewGuid();
                    string slogan = "Slogan " + Guid.NewGuid();
                    string pitch = "Pitch " + Guid.NewGuid();

                    var userid = userTable.CreateUser( ctx, 1, userName, firstName, lastName );

                    if( i < 25 )
                    {
                        userType = 0;
                        await userTimed.CreateOrUpdateTimedUserAsync( ctx, userType, id, userid );
                        group.AddUser( ctx, 1, 5, userid, true );


                    }
                    else if( i < 50 )
                    {
                        userType = 1;
                        TimedUserStruct timedStudent = await userTimed.CreateOrUpdateTimedUserAsync( ctx, userType, id, userid );
                        timedStudents.Add( timedStudent );
                        var ProjectCreate = await projectStudent.CreateProjectStudent( ctx, 1, 2, "name", 1, "a;b;c", logo, slogan, pitch, 1, "I" );
                        group.AddUser( ctx, 1, 6, userid, true );

                    }

                    else if( i < 75 )
                    {
                        userType = 2;
                        TimedUserStruct timedStaffMember =  await userTimed.CreateOrUpdateTimedUserAsync( ctx, userType, id, userid );
                        timedStaffMembers.Add( timedStaffMember );
                        group.AddUser( ctx, 1, 7, userid, true );


                    }
                    else if( i < 100 )
                    {
                        userType = 3;
                        TimedUserStruct timedJury = await userTimed.CreateOrUpdateTimedUserAsync( ctx, userType, id, userid );
                        timedJurys.Add( timedJury );
                        group.AddUser( ctx, 1, 8, userid, true );


                    }



                }

            }


        }

        public async Task create_zone()
        {
            var period = TestHelper.StObjMap.StObjs.Obtain<TimePeriodTable>();
            var group = TestHelper.StObjMap.StObjs.Obtain <CustomGroupTable>();

            using( var ctx = new SqlStandardCallContext() )
            {
                DateTime begdate = new DateTime(2019, 03, 01);
                DateTime enddate = new DateTime( 2019, 07, 31 );
                await period.CreateTimePeriodAsync( ctx, 1, begdate, enddate, "S", 4 );

                begdate = begdate.AddMonths( 6 );
                enddate = begdate.AddMonths( 6 );
                await period.CreateTimePeriodAsync( ctx, 1, begdate, enddate, "S", 4 );

                begdate = begdate.AddMonths( 6 );
                enddate = begdate.AddMonths( 6 );
                await period.CreateTimePeriodAsync( ctx, 1, begdate, enddate, "S", 4 );

                await group.MoveGroupAsync( ctx, 1, 5, 22, GroupMoveOption.AutoUserRegistration);
                await group.MoveGroupAsync( ctx, 1, 6, 22, GroupMoveOption.AutoUserRegistration );
                await group.MoveGroupAsync( ctx, 1, 7, 22, GroupMoveOption.AutoUserRegistration );
                await group.MoveGroupAsync( ctx, 1, 8, 22, GroupMoveOption.AutoUserRegistration );
                await group.MoveGroupAsync( ctx, 1, 9, 22, GroupMoveOption.AutoUserRegistration );
                await group.MoveGroupAsync( ctx, 1, 10, 22, GroupMoveOption.AutoUserRegistration );
                await group.MoveGroupAsync( ctx, 1, 11, 22, GroupMoveOption.AutoUserRegistration );
                await group.MoveGroupAsync( ctx, 1, 12, 22, GroupMoveOption.AutoUserRegistration );
                await group.MoveGroupAsync( ctx, 1, 13, 22, GroupMoveOption.AutoUserRegistration );
                await group.MoveGroupAsync( ctx, 1, 14, 22, GroupMoveOption.AutoUserRegistration );


            }

        }
    }
}
