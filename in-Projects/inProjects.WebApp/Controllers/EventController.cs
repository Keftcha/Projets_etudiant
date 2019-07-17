using CK.Auth;
using CK.Core;
using CK.SqlServer;
using CK.SqlServer.Setup;
using inProjects.Data;
using inProjects.Data.Data.Event;
using inProjects.Data.Data.Group;
using inProjects.Data.Data.Period;
using inProjects.Data.Data.TimedUser;
using inProjects.Data.Queries;
using inProjects.ViewModels;
using inProjects.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    public class EventController : Controller
    {
        readonly IStObjMap _stObjMap;
        readonly IAuthenticationInfo _authenticationInfo;
        readonly List<string> _typeEvents;

        public EventController( IStObjMap stObjMap, IAuthenticationInfo authenticationInfo )
        {
            _stObjMap = stObjMap;
            _authenticationInfo = authenticationInfo;
            _typeEvents = GetEventType();

        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            var sqlDataBase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();

            using( var ctx = new SqlStandardCallContext() )
            {
                GroupQueries groupQueries = new GroupQueries( ctx, sqlDataBase );
                EventQueries eventQueries = new EventQueries( ctx, sqlDataBase );

                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );

                AclQueries aclQueries = new AclQueries( ctx, sqlDataBase );

                if( !await aclQueries.VerifyGrantLevelByUserId( 112, await aclQueries.GetAclIdBySchoolId( groupData.ParentZoneId), userId, Operator.SuperiorOrEqual ))
                {
                    Result result = new Result( Status.Unauthorized, "Vous n'etes pas autorisé à utiliser cette fonctionnalité !" );
                    return this.CreateResult( result );
                }

                List<EventData> events = await eventQueries.GetAllEventsByIdSchool( groupData.ParentZoneId );

                return Ok( events );


            }
        }

        internal List<string> GetEventType()
        {
            using( StreamReader stream = new StreamReader( "eventsSelect.json" ) )
            {
                using( JsonTextReader jsonReader = new JsonTextReader( stream ) )
                {
                    JToken json = JToken.Load( jsonReader );

                    IEnumerable<string> authorizeString =
                     from p in json["Events"]
                     select (string)p;

                    List<string> List = authorizeString.ToList();


                    return List;

                }
            }

        }

        [HttpGet("GetTypeEvents")]
        public async Task<IActionResult> GetTypeEvents()
        {
            return Ok( _typeEvents );
        }

        [HttpPost("UpdateEvents")]
        public async Task<IActionResult> UpdateEvents([FromBody] SchoolEventChangeModel model)
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            var sqlDataBase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();

            using( var ctx = new SqlStandardCallContext() )
            {
                PeriodServices periodServices = new PeriodServices();
                GroupQueries groupQueries = new GroupQueries( ctx, sqlDataBase );
                EventQueries eventQueries = new EventQueries( ctx, sqlDataBase );

                if( !await periodServices.CheckPeriodGivenDate( _stObjMap, _authenticationInfo, model.BegDate, model.EndDate ) )
                {
                    Result result = new Result( Status.BadRequest, "Ces Dates ne sont pas comprises dans la periode actuel" );
                    return this.CreateResult( result );
                }

                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );

                AclQueries aclQueries = new AclQueries( ctx, sqlDataBase );

                if( !await aclQueries.VerifyGrantLevelByUserId( 112, await aclQueries.GetAclIdBySchoolId( groupData.ParentZoneId ), userId, Operator.SuperiorOrEqual ) )
                {
                    Result result = new Result( Status.Unauthorized, "Vous n'etes pas autorisé à utiliser cette fonctionnalité !" );
                    return this.CreateResult( result );
                }

                await eventQueries.UpdateEvent( model.EventId, model.Name, model.BegDate, model.EndDate );

                List<EventData> events = await eventQueries.GetAllEventsByIdSchool( groupData.ParentZoneId );

                return Ok( events );


            }
        }

        [HttpPost( "CreateEvents" )]
        public async Task<IActionResult> CreateEvents( [FromBody] SchoolEventChangeModel model )
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            var sqlDataBase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            var eventTable = _stObjMap.StObjs.Obtain<EventSchoolTable>();

            using( var ctx = new SqlStandardCallContext() )
            {
                PeriodServices periodServices = new PeriodServices();
                GroupQueries groupQueries = new GroupQueries( ctx, sqlDataBase );
                TimedPeriodQueries timedPeriodQueries = new TimedPeriodQueries( ctx, sqlDataBase );
                EventQueries eventQueries = new EventQueries( ctx, sqlDataBase );

                if(!await periodServices.CheckPeriodGivenDate( _stObjMap,_authenticationInfo,model.BegDate,model.EndDate ) )
                {
                    Result result = new Result( Status.BadRequest, "Ces Dates ne sont pas comprises dans la periode actuel" );
                    return this.CreateResult( result );
                }

                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );

                AclQueries aclQueries = new AclQueries( ctx, sqlDataBase );

                if( !await aclQueries.VerifyGrantLevelByUserId( 112, await aclQueries.GetAclIdBySchoolId( groupData.ParentZoneId ), userId, Operator.SuperiorOrEqual ) )
                {
                    Result result = new Result( Status.Unauthorized, "Vous n'etes pas autorisé à utiliser cette fonctionnalité !" );
                    return this.CreateResult( result );
                }

                if( !model.IsOther )
                {

                    model.Name += "-" + await timedPeriodQueries.GetGroupNameActualPeriod( groupData.ZoneId, groupData.ParentZoneId );
                }

                EventStruct eventResult = await eventTable.CreateEvent( ctx, model.Name, model.BegDate, model.EndDate, groupData.ZoneId, userId );

                if(eventResult.Status == 1 )
                {
                    Result result = new Result( Status.Unauthorized, "Cette evenement existe deja dans votre école !" );
                    return this.CreateResult( result );
                }
                List<EventData> events = await eventQueries.GetAllEventsByIdSchool( groupData.ParentZoneId );

                return Ok( events );


            }
        }

        [HttpDelete( "DeleteEvent" )]
        public async Task<IActionResult> DeleteEvent(int EventId)
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            var sqlDataBase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            var eventTable = _stObjMap.StObjs.Obtain<EventSchoolTable>();

            using( var ctx = new SqlStandardCallContext() )
            {
                AclQueries aclQueries = new AclQueries( ctx, sqlDataBase );
                GroupQueries groupQueries = new GroupQueries( ctx, sqlDataBase );

                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );

                if( !await aclQueries.VerifyGrantLevelByUserId( 112, await aclQueries.GetAclIdBySchoolId( groupData.ParentZoneId ), userId, Operator.SuperiorOrEqual ) )
                {
                    Result result = new Result( Status.Unauthorized, "Vous n'etes pas autorisé à utiliser cette fonctionnalité !" );
                    return this.CreateResult( result );
                }

               EventStruct eventResult = await eventTable.DeleteEvent( ctx, userId, EventId );

               if(eventResult.Status == 1 )
                {
                    Result result = new Result( Status.BadRequest, "Cette évenement n'existe plus ou n'a jamais existé ! " );
                    return this.CreateResult( result );
                }

                return Ok();
               

            }
        }

        [HttpGet( "GetEventSchoolByName" )]
        public async Task<IActionResult> GetEventSchoolByName(string eventName, string schoolName)
        {
            var sqlDataBase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            int userId = _authenticationInfo.ActualUser.UserId;

            using( var ctx = new SqlStandardCallContext() )
            {
                EventQueries eventQueries = new EventQueries( ctx, sqlDataBase );
                GroupQueries groupQueries = new GroupQueries( ctx, sqlDataBase );
                TimedPeriodQueries timedPeriodQueries = new TimedPeriodQueries( ctx, sqlDataBase );

                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );
                EventData eventData;

                if(groupData == null )
                {
                    int idSchool = await groupQueries.GetIdSchoolByName( schoolName );
                    PeriodData periodData = await timedPeriodQueries.GetLastPeriodBySchool( idSchool );
                    eventData = await eventQueries.GetEventSchoolBylNameAndPeriodId( eventName, periodData.ChildId );

                }
                else
                {
                    eventData = await eventQueries.GetEventSchoolBylNameAndPeriodId( eventName, groupData.ZoneId );

                }


                return Ok( eventData );
            }
        }




        //public async Task<bool> IsUserHasRight( SqlStandardCallContext ctx, SqlDefaultDatabase sqlDataBase,int userId  )
        //{
        //    GroupQueries groupQueries = new GroupQueries( ctx, sqlDataBase );
        //    EventQueries eventQueries = new EventQueries( ctx, sqlDataBase );

        //    GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );

        //    AclQueries aclQueries = new AclQueries( ctx, sqlDataBase );

        //    if( !await aclQueries.VerifyGrantLevelByUserId( 112, await aclQueries.GetAclIdBySchoolId( groupData.ParentZoneId ), userId, Operator.SuperiorOrEqual ) )
        //    {
        //        return false;
        //    }else
        //    {
        //        return true;
        //    }
        //}
    }
}
