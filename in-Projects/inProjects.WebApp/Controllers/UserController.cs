using CK.Core;
using Microsoft.AspNetCore.Mvc;
using inProjects.ViewModels;
using inProjects.Data.Queries;
using CK.Auth;
using inProjects.Data.Data.Group;
using System.Collections.Generic;
using inProjects.Data.Data.TimedUser;
using System.Linq;
using System.Threading.Tasks;
using CK.SqlServer;
using CK.SqlServer.Setup;
using inProjects.WebApp.Services.CSV;
using inProjects.Data;
using inProjects.WebApp.Services;
using inProjects.EmailJury;

namespace inProjects.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class UserController: Controller
    {
        readonly IStObjMap _stObjMap;
        readonly IAuthenticationInfo _authenticationInfo;
        private readonly Emailer _emailer;
        public UserController(IStObjMap stObjMap, IAuthenticationInfo authenticationInfo, Emailer emailer )
        {
            _stObjMap = stObjMap;
            _authenticationInfo = authenticationInfo;
            _emailer = emailer;
        }

        [HttpPost( "getUserList" )]

        public async Task<List<TimedStudentData>> GetAllUserByPeriod([FromBody] BddInfoModel model)
        {
            var sqlDatabase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            IEnumerable<TimedStudentData> studentList = new List<TimedStudentData>();
            List<TimedStudentData> timedStudentDatas = new List<TimedStudentData>();
            int userId = _authenticationInfo.ActualUser.UserId;


            using( var ctx = new SqlStandardCallContext() )
            {
                AclQueries aclQueries = new AclQueries( ctx, sqlDatabase );

                if(!await aclQueries.VerifyGrantLevelByUserId(112, 9, userId, Operator.SuperiorOrEqual ) )
                {
                    Result result = new Result( Status.Unauthorized, "Vous n'etes pas autorisé à utiliser cette fonctionnalité !" );
                }


                TimedPeriodQueries timedPeriodQueries = new TimedPeriodQueries( ctx, sqlDatabase );
                GroupQueries groupQueries = new GroupQueries( ctx, sqlDatabase );
                TimedUserQueries timedUserQueries = new TimedUserQueries( ctx, sqlDatabase );


                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );
               //PeriodData periodData = await timedPeriodQueries.GetLastPeriodBySchool( groupData.ZoneId );
                IEnumerable<GroupData> groupList = await groupQueries.GetAllGroupByPeriod( groupData.ZoneId );

                List<GroupData> groupFinal = groupList.ToList();
                for( int i = 0; i < groupFinal.Count; i++ )
                {
                        studentList = await timedUserQueries.GetAllStudentInfosByGroup( groupFinal[i].GroupId, model.TableName, model.TableId );
                        timedStudentDatas.AddRange( studentList );
                }

            }

            var duplicates = timedStudentDatas
                .GroupBy( x => x.UserId )
                .Where( g => g.Count() > 1 )
                .Select( g => g.Select( gg => gg ) );

            var single = timedStudentDatas
                .GroupBy( x => x.UserId )
                .Where( g => g.Count() == 1 )
                .SelectMany( g => g.Select( gg => gg ) );

            List<TimedStudentData> timedStudentDatasClean = new List<TimedStudentData>();
            timedStudentDatasClean = single.ToList();
            for( var i = 0; i < duplicates.Count(); i++ )
            {
                var a = duplicates.ElementAt( i );

                if(a.ElementAt( 0 ).GroupName == "S01"  || a.ElementAt(0).GroupName == "S02")
                {
                    a.ElementAt( 0 ).GroupName = a.ElementAt( 0 ).GroupName;
                }else if (a.ElementAt(1).GroupName == "SR")
                {
                    a.ElementAt( 0 ).GroupName = a.ElementAt( 0 ).GroupName + " - " + a.ElementAt( 1 ).GroupName;
                }
                else
                {
                    a.ElementAt( 0 ).GroupName = a.ElementAt( 1 ).GroupName + " - " + a.ElementAt( 0 ).GroupName;
                }
                timedStudentDatasClean.Add( a.ElementAt( 0 ));
            }
;

            return timedStudentDatasClean;

        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> AddStudentListCsv( string type )
        {
            var file = Request.Form.Files[0];

            int userId = _authenticationInfo.ActualUser.UserId;
            var sqlDatabase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            PeriodServices periodServices = new PeriodServices();

            using( var ctx = new SqlStandardCallContext() )
            {
                GroupQueries groupQueries = new GroupQueries( ctx, sqlDatabase );
                AclQueries aclQueries = new AclQueries( ctx, sqlDatabase );
                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );

                // User must have the rights to do this action
                //if( await aclQueries.VerifyGrantLevelByUserId( 112, await aclQueries.GetAclIdBySchoolId( groupData.ParentZoneId ), userId, Operator.SuperiorOrEqual ) == false )
                //{
                //    Result result = new Result( Status.Unauthorized, "Vous n'etes pas autorisé à utiliser cette fonctionnalité !" );
                //    return this.CreateResult( result );
                //}

                bool isInPeriod = await periodServices.CheckInPeriod( _stObjMap, _authenticationInfo );

                // The school in wich the user is need to be in a period when the action is done
                if( !isInPeriod )
                {
                    Result result = new Result( Status.Unauthorized, "A la date d'aujourd'hui votre etablissement n'est dans une aucune periode" );
                    return this.CreateResult( result );
                }

                if(type =="student" || type == "staffMember" )
                {
                    CsvStudentMapping<UserList> csvStudentMapping = new CsvStudentMapping<UserList>( _emailer );
                    List<UserList> studentResult = await csvStudentMapping.CSVReader( file );
                    await csvStudentMapping.UserParser( studentResult, _stObjMap, _authenticationInfo, type );
                }
                else if (type == "projectNumber" )
                {
                    CsvStudentMapping<ProjectNumbers> csvStudentMapping = new CsvStudentMapping<ProjectNumbers>( _emailer );
                    List<ProjectNumbers> projectNumbers = await csvStudentMapping.CSVReaderProjectNumber( file );
                    await csvStudentMapping.ForumNumberParser( _stObjMap, projectNumbers, _authenticationInfo );
                }
                else if (type == "jury" )
                {
                    CsvStudentMapping<JuryInfos> csvStudentMapping = new CsvStudentMapping<JuryInfos>( _emailer );
                    List<JuryInfos> result = await csvStudentMapping.CSVReaderProjectNumber( file );
                    await csvStudentMapping.AssignProjectToJury( _stObjMap, _authenticationInfo, result, type );
                    

                }

            }

            return Ok();
        }


    }
}
