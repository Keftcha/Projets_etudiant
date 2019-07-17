using CK.Auth;
using CK.Core;
using CK.SqlServer;
using CK.SqlServer.Setup;
using inProjects.Data;
using inProjects.Data.Data.Period;
using inProjects.Data.Data.TimePeriod;
using inProjects.Data.Queries;
using inProjects.ViewModels;
using inProjects.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    public class PeriodController : Controller
    {
        readonly IStObjMap _stObjMap;
        readonly IAuthenticationInfo _authenticationInfo;

        public PeriodController( IStObjMap stObjMap, IAuthenticationInfo authenticationInfo )
        {
            _stObjMap = stObjMap;
            _authenticationInfo = authenticationInfo;
        }

        [HttpPost( "createPeriod" )]
        public async Task<IActionResult> CreatePeriod( [FromBody] CreatePeriodModel createPeriodModel )
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            var sqlDatabase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            var group = _stObjMap.StObjs.Obtain<CustomGroupTable>();
            var timePeriod = _stObjMap.StObjs.Obtain<TimePeriodTable>();
            var timedUser = _stObjMap.StObjs.Obtain<TimedUserTable>();

            using( var ctx = new SqlStandardCallContext() )
            {
                AclQueries aclQueries = new AclQueries( ctx, sqlDatabase );

                //if( await aclQueries.VerifyGrantLevelByUserId( 112, await aclQueries.GetAclIdBySchoolId( createPeriodModel.idZone ), userId, Operator.SuperiorOrEqual ) == false )
                //{
                //    Result result = new Result( Status.Unauthorized, "Vous n'etes pas autorisé à utiliser cette fonctionnalité !" );
                //    return this.CreateResult( result );
                //}

                createPeriodModel.begDate =  createPeriodModel.begDate.AddDays( 1 );
                createPeriodModel.endDate = createPeriodModel.endDate.AddDays( 1 );

                var idPeriod = await timePeriod.CreateTimePeriodAsync( ctx, 1, createPeriodModel.begDate, createPeriodModel.endDate, createPeriodModel.Kind, createPeriodModel.idZone );
                if(idPeriod == 0 )
                {
                    Result result = new Result( Status.BadRequest, "Quelques choses s'est mal passé durant la création de periode." );
                    return this.CreateResult( result );
                }
                string groupName = createPeriodModel.begDate.Year +  "-0"  + createPeriodModel.begDate.Month ;

                await group.Naming.GroupRenameAsync( ctx, userId, idPeriod, groupName );

                int idGroupAdmin = 0;
                for( int i = 0; i < createPeriodModel.Groups.Count; i++ )
                {
                    int idGroup;

                    if( createPeriodModel.Groups[i].IsAlreadyPermanent == false && createPeriodModel.Groups[i].State == true )
                    {
                        idGroup = await group.CreateGroupAsync( ctx, userId, 4 );
                        await group.Naming.GroupRenameAsync( ctx, 17, idGroup, createPeriodModel.Groups[i].Name );
                    }

                    idGroup = await group.CreateGroupAsync( ctx, userId, idPeriod );
                    await group.Naming.GroupRenameAsync( ctx, 17, idGroup, createPeriodModel.Groups[i].Name );

                    if( createPeriodModel.Groups[i].Name == "Administration" ) idGroupAdmin = idGroup;
                }

                await group.AddUserAsync( ctx, 1, idGroupAdmin, userId, true );

                await timedUser.CreateOrUpdateTimedUserAsyncWithType( ctx, Data.TypeTimedUser.StaffMember, idPeriod, userId );
                return this.CreateResult( Result.Success() );

            }
        }
        [HttpGet("verifyActualPeriod")]
        public async Task<IActionResult> VerifyActualPeriod()
        {
            PeriodServices periodServices = new PeriodServices();

            return Ok( await periodServices.CheckInPeriod(_stObjMap,_authenticationInfo) );
        }

        [HttpGet( "getAllPeriod" )]
        public async Task<IActionResult> GetAllPeriod( int idZone )
        {
            var sqlDataBase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();

            using( var ctx = new SqlStandardCallContext() )
            {
                TimedPeriodQueries timedPeriod = new TimedPeriodQueries( ctx, sqlDataBase );
                DateTime dateNow = DateTime.UtcNow;
                List<PeriodData> period = await timedPeriod.GetAllPeriodsBySchool( idZone );
                return Ok( period );
            }

        }

        [HttpPost( "changeDateOfPeriod" )]
        public async Task<IActionResult> ChangeDateOfPeriod([FromBody] PeriodChangeDateModel periodChangeDateModel )
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            var sqlDataBase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            var group = _stObjMap.StObjs.Obtain<CustomGroupTable>();

            using( var ctx = new SqlStandardCallContext() )
            {
                TimedPeriodQueries timedPeriod = new TimedPeriodQueries( ctx, sqlDataBase );

                PeriodData period = await timedPeriod.GetPeriodById( periodChangeDateModel.idPeriod );

                await timedPeriod.UpdatePeriodDate( periodChangeDateModel.idPeriod, periodChangeDateModel.begDate, periodChangeDateModel.endDate );

                if( periodChangeDateModel.begDate.Year > period.BegDate.Year || periodChangeDateModel.begDate.Year < period.BegDate.Year )
                {
                    string groupName = periodChangeDateModel.begDate.Year + "-0" + periodChangeDateModel.begDate.Month;

                    await group.Naming.GroupRenameAsync( ctx, userId, periodChangeDateModel.idPeriod, groupName );

                }
                return Ok();
            }

        }

    }
}
