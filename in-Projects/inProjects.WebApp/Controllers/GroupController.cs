using CK.Auth;
using CK.Core;
using CK.SqlServer;
using CK.SqlServer.Setup;
using inProjects.Data.Data.Period;
using inProjects.Data.Data.TimedUser;
using inProjects.Data.Queries;
using inProjects.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    public class GroupController : Controller
    {
        readonly IStObjMap _stObjMap;
        readonly IAuthenticationInfo _authenticationInfo;

        public GroupController( IStObjMap stObjMap, IAuthenticationInfo authenticationInfo )
        {
            _stObjMap = stObjMap;
            _authenticationInfo = authenticationInfo;
        }

        [HttpGet("getAllGroupTemplate")]
        public async Task<List<GroupPeriod>> GetAllGroupTemplate(int idZone)
        {
            var sqlDataBase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();

            using( var ctx = new SqlStandardCallContext() )
            {
                GroupQueries groupQueries = new GroupQueries( ctx, sqlDataBase );

                List<string> listGroups = await groupQueries.GetAllGroupByZoneId( idZone );

                List<GroupPeriod> list = new List<GroupPeriod>();

                for( int i = 0; i < listGroups.Count; i++ )
                {
                    GroupPeriod gp = new GroupPeriod();
                    gp.Name = listGroups[i];
                    gp.State = true;
                    gp.IsAlreadyPermanent = true;
                    list.Add( gp );
                }

                return list;
            }
        }
        [HttpGet( "getGroupUserAccessPanel" )]
        public async Task<List<string>> GetGroupUserAccessPanel(int idZone)
        {
            List<string> listGroupToReturn = new List<string>();
            int userId = _authenticationInfo.ActualUser.UserId;
            var sqlDataBase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            if( userId == 0 )
            {
                listGroupToReturn.Add( "Anon" );
                return listGroupToReturn;
            }

            listGroupToReturn.Add( "User" );

            using( var ctx = new SqlStandardCallContext() )
            {
                TimedPeriodQueries timedPeriod = new TimedPeriodQueries( ctx, sqlDataBase );
                TimedUserQueries timedUser = new TimedUserQueries( ctx, sqlDataBase );
                GroupQueries groupsQueries = new GroupQueries( ctx, sqlDataBase );
                UserQueries userQueries = new UserQueries( ctx, sqlDataBase );
                DateTime dateNow = DateTime.UtcNow;
                List<PeriodData> periods = await timedPeriod.GetNumberWishPeriodBySchool( idZone, 2 );
                PeriodData period = null;


                for( int i = 0; i < periods.Count; i++ )
                {
                    if(periods[i].BegDate.DayOfYear <= dateNow.DayOfYear && periods[i].EndDate.DayOfYear >= dateNow.DayOfYear )
                    {
                        period = periods[i];
                        break;
                    }
                }

                if( period == null ) period = periods[0];
                TimedUserData timedUserData = await timedUser.GetTimedUser( userId, period.ChildId );

                if(timedUserData == null )
                {
                    int adminIdZoneSchool = await groupsQueries.GetSpecificIdGroupByZoneIdAndGroupName( idZone, "Administration" );

                    if( await userQueries.VerifyGroupUser( userId, adminIdZoneSchool) != 0)
                    {
                        listGroupToReturn.Add( "Administration" );
                    }

                    return listGroupToReturn;
                }

                listGroupToReturn = await timedUser.getWhichCat( timedUserData.TimedUserId,listGroupToReturn );
                if(!listGroupToReturn.Contains("StaffMember"))
                {
                    return listGroupToReturn;
                }

                List<string> listGroupsOfTimedUser = new List<string>();

                listGroupsOfTimedUser = await groupsQueries.GetAllGroupOfTimedUserByPeriod( period.ChildId, timedUserData.TimedUserId );

                for( int i = 0; i < listGroupsOfTimedUser.Count; i++ )
                {
                    if(listGroupsOfTimedUser[i] == "Administration" )
                    {
                        listGroupToReturn.Add( "Administration" );
                    }
                    else if(listGroupsOfTimedUser[i] == "Teacher" )
                    {
                        listGroupToReturn.Add( "Teacher" );

                    }

                }

                if( !listGroupToReturn.Contains( "Administration" ) )
                {
                    int adminIdZoneSchool = await groupsQueries.GetSpecificIdGroupByZoneIdAndGroupName( idZone, "Administration" );

                    if( await userQueries.VerifyGroupUser( userId, adminIdZoneSchool ) != 0 )
                    {
                        listGroupToReturn.Add( "Administration" );
                    }
                }
                

                return listGroupToReturn;
            }
        }
    }
}
