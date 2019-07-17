using CK.Auth;
using CK.Core;
using CK.SqlServer;
using CK.SqlServer.Setup;
using inProjects.Data;
using inProjects.Data.Data.Group;
using inProjects.Data.Data.School;
using inProjects.Data.Data.TimedUser;
using inProjects.Data.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    public class SchoolController : Controller
    {
        readonly IStObjMap _stObjMap;
        readonly IAuthenticationInfo _authenticationInfo;

        public SchoolController( IStObjMap stObjMap, IAuthenticationInfo authenticationInfo )
        {
            _stObjMap = stObjMap;
            _authenticationInfo = authenticationInfo;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchools()
        {
            var sqlDatabase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            using( var ctx = new SqlStandardCallContext() )
            {
                SchoolQueries schoolQueries = new SchoolQueries( ctx, sqlDatabase );
                List<SchoolData> schools = await schoolQueries.GetAllSchool();

                return Ok( schools );
            }
        }

        [HttpGet("getIdSchoolOfUser")]
        public async Task<IActionResult> GetIdSchoolOfUser()
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            var sqlDatabase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            using( var ctx = new SqlStandardCallContext() )
            {
                SchoolQueries schoolQueries = new SchoolQueries( ctx, sqlDatabase );
                TimedUserQueries timedUserQueries = new TimedUserQueries( ctx, sqlDatabase );
                GroupQueries groupQueries = new GroupQueries( ctx, sqlDatabase );

                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );

                if( groupData == null || groupData.ParentZoneId == 0 )
                {
                    //if user not in school default by school id In'TECH
                   // int idSchool = await groupQueries.GetIdSchoolByName( "IN'TECH" );
                    return Ok( groupData.ZoneId );
                }

                return Ok(groupData.ParentZoneId);
            }
        }
    }
}
