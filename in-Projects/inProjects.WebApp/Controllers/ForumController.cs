using CK.Core;
using OfficeOpenXml;
using inProjects.Data.Data.Period;
using inProjects.Data.Data.ProjectStudent;
using inProjects.Data.Queries;
using inProjects.Data.ForumsPlan;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CK.SqlServer;
using CK.SqlServer.Setup;
using inProjects.Data.Data.Group;
using CK.Auth;
using System.Linq;
using inProjects.Data;
using inProjects.WebApp.Services.Excel;
using System.IO;

namespace inProjects.WebApp.Controllers
{
    [Route( "api/[controller]" )]
    public class ForumController : Controller
    {
        readonly IStObjMap _stObjMap;
        readonly IAuthenticationInfo _authenticationInfo;

        public ForumController( IStObjMap stObjMap, IAuthenticationInfo authenticationInfo )
        {
            _stObjMap = stObjMap;
            _authenticationInfo = authenticationInfo;
        }

        [HttpGet("GetPlan")]
        public async Task<Plan> InitPlan()
        {
            Plan plan = new Plan( 50, 30 );
            await plan.PopulateWithClassRooms();

            return plan;
        }

        [HttpGet("GetProjects")]
        public async Task<List<Project>> GetAllProjects()
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            var sqlDatabase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            List<Project> projectList = new List<Project>();

            using( var ctx = new SqlStandardCallContext() )
            {
                ProjectQueries projectQueries = new ProjectQueries( ctx, sqlDatabase );
                GroupQueries groupQueries = new GroupQueries( ctx, sqlDatabase );
                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );
                IEnumerable<ProjectData> projects = await projectQueries.GetAllProjectByForum( groupData.ZoneId );
                foreach( ProjectData project in projects )
                {
                    List<string> listGroups = await projectQueries.GetGroupsOfProject( project.ProjectStudentId );
                    listGroups = listGroups.FindAll( x => x.StartsWith( "S0" ) || x == "IL" || x == "SR" );
                    project.Semester = string.Join( "-", listGroups.ToArray() );
                    if( listGroups[0] == "IL" ) project.Semester = string.Join( "-", listGroups.ToArray().Reverse() );
                    Project p = new Project( project.ProjectStudentId, project.Name,
                        project.Semester, project.CoordinatesX, project.CoordinatesY, project.ClassRoom, project.Height, project.Width, project.ForumNumber );
                    projectList.Add( p );
                }
            }
            return projectList;
        }

        [HttpPost("SavePlan")]
        public async Task<List<int>> SavePlan([FromBody] Project[] plan)
        {
            var sqlDatabase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            using( var ctx = new SqlStandardCallContext() )
            {
                ForumQueries forumQueries = new ForumQueries( ctx, sqlDatabase );
                List<int> results = await forumQueries.SavePlan( plan );
                return results;
            }
        }

        [HttpGet( "DownloadExcel" )]
        public async Task<IActionResult> DownloadExcel(  )
        {
            var sqlDatabase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            ExcelUtilis excel = new ExcelUtilis();
            int userId = _authenticationInfo.ActualUser.UserId;

            using( var ctx = new SqlStandardCallContext() )
            {
                GroupQueries groupQueries = new GroupQueries( ctx, sqlDatabase );
                ProjectQueries projectQueries = new ProjectQueries( ctx, sqlDatabase );

                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );

                AclQueries aclQueries = new AclQueries( ctx, sqlDatabase );

                if( await aclQueries.VerifyGrantLevelByUserId( 112, await aclQueries.GetAclIdBySchoolId( groupData.ParentZoneId ), userId, Operator.SuperiorOrEqual ) == false )
                {
                    Result result = new Result( Status.Unauthorized, "Vous n'etes pas autorisé à utiliser cette fonctionnalité !" );
                    return this.CreateResult( result );
                }
                List<ProjectInfosJuryData> projectInfosJuries = await projectQueries.getAllProjectsGrade( groupData.ZoneId );

                List<ProjectForumResultData> allProjectsForumResult = GetProjectsOfForum( projectInfosJuries );


                byte[] excelToArray =  await excel.CreateExcel( allProjectsForumResult,projectQueries );

                return File( excelToArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" );
            }
        }

        [HttpGet( "getAllGradeProjects" )]
        public async Task<IActionResult> GetAllGradeProject()
        {
            SqlDefaultDatabase db = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            int userId = _authenticationInfo.ActualUser.UserId;

            using( var ctx = new SqlStandardCallContext() )
            {
                ProjectQueries projectQueries = new ProjectQueries( ctx, db );
                TimedPeriodQueries timedPeriodQueries = new TimedPeriodQueries( ctx, db );
                TimedUserQueries timedUserQueries = new TimedUserQueries( ctx, db );
                UserQueries userQueries = new UserQueries( ctx, db );
                GroupQueries groupQueries = new GroupQueries( ctx, db );

                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );

                AclQueries aclQueries = new AclQueries( ctx, db );

                if( await aclQueries.VerifyGrantLevelByUserId( 112, await aclQueries.GetAclIdBySchoolId( groupData.ParentZoneId ), userId, Operator.SuperiorOrEqual ) == false )
                {
                    Result result = new Result( Status.Unauthorized, "Vous n'etes pas autorisé à utiliser cette fonctionnalité !" );
                    return this.CreateResult( result );
                }

                List<ProjectInfosJuryData> projects = await projectQueries.getAllProjectsGrade( groupData.ZoneId );

                List<ProjectForumResultData> listToSend = GetProjectsOfForum( projects );
               
                return Ok( listToSend );
            }

        }

        [HttpGet( "getAllPublicNote" )]
        public async Task<IActionResult> GetAllPublicNote()
        {
            SqlDefaultDatabase db = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            int userId = _authenticationInfo.ActualUser.UserId;

            using( var ctx = new SqlStandardCallContext() )
            {
                ProjectQueries projectQueries = new ProjectQueries( ctx, db );
                TimedPeriodQueries timedPeriodQueries = new TimedPeriodQueries( ctx, db );
                TimedUserQueries timedUserQueries = new TimedUserQueries( ctx, db );
                UserQueries userQueries = new UserQueries( ctx, db );
                GroupQueries groupQueries = new GroupQueries( ctx, db );

                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );

                List<ProjectForumResultData> projects = await projectQueries.GetAllPublicsNoteByTimedPeriodId( groupData.ZoneId );

                return Ok( projects );
            }

        }

        internal List<ProjectForumResultData> GetProjectsOfForum( List<ProjectInfosJuryData> projects )
        {
            int diviseur = 0;
            int count = 0;
            ProjectInfosJuryData oldProject = new ProjectInfosJuryData();
            List<double> gradeOfProject = new List<double>();
            List<ProjectForumResultData> forumsResult = new List<ProjectForumResultData>();
            ProjectForumResultData project = new ProjectForumResultData();
            Dictionary<string, double> individualGrade = new Dictionary<string, double>();
            List<int> jurysId = new List<int>();

            foreach( var item in projects )
            {

                if( item.ProjectStudentId != oldProject.ProjectStudentId )
                {
                    double moyenne = 0;
                    double total = 0;
                    total = gradeOfProject.Take( gradeOfProject.Count ).Sum();
                    if( diviseur > 0 ) moyenne = Math.Round( total / diviseur, 2 );
                    project.Average = moyenne;
                    diviseur = 0;
                    project.IndividualGrade = individualGrade;
                    project.JurysId = jurysId;
                    individualGrade = new Dictionary<string, double>();
                    jurysId = new List<int>();
                    gradeOfProject = new List<double>();
                    forumsResult.Add( project );
                    project = new ProjectForumResultData
                    {
                        Name = item.ProjectName,
                        ProjectId = item.ProjectStudentId,
                        ClassRoom = item.ClassRoom,
                        ForumNumber = item.ForumNumber
                        

                    };
                }

                if( item.Grade > 0 )
                {
                    gradeOfProject.Add( item.Grade );
                    diviseur++;
                }

                oldProject = item;
                count++;
                individualGrade.Add( item.JuryName, item.Grade );
                jurysId.Add( item.JuryId );
                if( item.IsBlocked == true ) project.IsBlocked = true;

                if( count == projects.Count )
                {
                    double moyenne = 0;
                    double total = 0;
                    total = gradeOfProject.Take( gradeOfProject.Count ).Sum();
                    if( diviseur > 0 ) moyenne = Math.Round( total / diviseur, 2 );
                    project.Average = moyenne;
                    diviseur = 0;
                    project.IndividualGrade = individualGrade;
                    project.JurysId = jurysId;
                    individualGrade = new Dictionary<string, double>();
                    forumsResult.Add( project );

                }


            }
            forumsResult.RemoveAt( 0 );
            List<ProjectForumResultData> listToSend = forumsResult.OrderByDescending( x => x.Average ).ToList();
            return listToSend;

        }


    }
}
