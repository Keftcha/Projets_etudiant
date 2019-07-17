using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using CK.Auth;
using CK.Core;
using CK.SqlServer;
using CK.SqlServer.Setup;
using inProjects.Data;
using inProjects.Data.Data.Group;
using inProjects.Data.Data.Period;
using inProjects.Data.Data.ProjectStudent;
using inProjects.Data.Data.TimedUser;
using inProjects.Data.Data.User;
using inProjects.Data.Queries;
using inProjects.Data.Res.Model;
using inProjects.TomlHelpers;
using inProjects.ViewModels;
using inProjects.WebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Linq;

namespace inProjects.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController: Controller
    {
        readonly IStObjMap _stObjMap;
        readonly IAuthenticationInfo _authenticationInfo;
        readonly List<double> _selectorInt;

        public ProjectController(IStObjMap stObjMap, IAuthenticationInfo authenticationInfo)
        {
            _stObjMap = stObjMap;
            _authenticationInfo = authenticationInfo;
            _selectorInt = GetSelectorInt();
        }

        private List<double> GetSelectorInt()
        {
            double x = 0;
            List<double> list = new List<double>();
            while(x <= 20)
            {
                list.Add( x );
                x += 0.25;
            }

            return list;
        }


        [HttpGet( "getSelectorGrade" )]
        public async Task<List<double>> GetSelectorGrade( )
        {
            return _selectorInt;
        }

        [HttpPost("submitProject")]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitProject([FromBody] SubmitProjectModel project)
        {
            ProjectStudentTable projectTable = _stObjMap.StObjs.Obtain<ProjectStudentTable>();           
            SqlDefaultDatabase db = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            CustomGroupTable groupTable = _stObjMap.StObjs.Obtain<CustomGroupTable>();
            ProjectUrlTable projectUrl = _stObjMap.StObjs.Obtain<ProjectUrlTable>();

           return Ok(await TomlHelpers.TomlHelpers.RegisterProject(
                project.Link,
                project.ProjectType,
                projectTable,
                project.UserId,
                db,
                groupTable,
                _stObjMap,
                projectUrl
            ));
        }

        [HttpGet( "getInfosProject" )]
        [AllowAnonymous]
        public async Task<IActionResult> GetInfosProject( int idProject )
        {
            ProjectStudentTable projectTable = _stObjMap.StObjs.Obtain<ProjectStudentTable>();
            SqlDefaultDatabase db = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();

            using( var ctx = new SqlStandardCallContext() )
            {
                ProjectQueries projectQueries = new ProjectQueries( ctx, db );

                ProjectDetailsData projectDetails = new ProjectDetailsData
                {
                    Project = await projectQueries.GetDetailProject( idProject ),

                    Students = await projectQueries.GetAllUsersOfProject(idProject)
                };

                List<string> listGroups = await projectQueries.GetGroupsOfProject( idProject);

                listGroups = listGroups.FindAll( x => x.StartsWith( "S0" ) || x == "IL" || x == "SR" );
                string groups = listGroups[0];

                if( listGroups.Count >= 2 )
                {
                    for( int i = 1; i < listGroups.Count; i++ )
                    {
                        groups += "-" + listGroups[i];
                    }
                }
               

                projectDetails.Project.Semester = groups;

                return Ok( projectDetails );

            }

        }

        [HttpGet( "verifyProjectFav" )]
        public async Task<IActionResult> VerifyProjectFav( int idProject )
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            var sqlDataBase = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();


            using( var ctx = new SqlStandardCallContext() )
            {
                ProjectQueries projectQueries = new ProjectQueries( ctx, sqlDataBase );

                bool exist = await projectQueries.IsProjectFav( idProject, userId );

                return Ok( exist );
            }
        }

        [HttpGet( "favProject" )]
        public async Task<IActionResult> FavProject( int idProject)
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            UserFavProjectTable favTable = _stObjMap.StObjs.Obtain<UserFavProjectTable>();
            SqlDefaultDatabase db = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();

            using( var ctx = new SqlStandardCallContext() )
            {
               await favTable.FavOrUnfavProject( ctx, userId, idProject );

               return Ok( );

            }

        }

        [HttpGet("getAllProjects")]
        public async Task<IEnumerable<AllProjectInfoData>> GetAllProjects()
        {
            SqlDefaultDatabase db = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            int userId = _authenticationInfo.ActualUser.UserId;

            using( var ctx = new SqlStandardCallContext() )
            {
                ProjectQueries projectQueries = new ProjectQueries( ctx, db );
                UserQueries userQueries = new UserQueries( ctx, db );
                GroupQueries groupQueries = new GroupQueries( ctx, db );

                IEnumerable<AllProjectInfoData> projectData = await projectQueries.GetAllProject(userId);
                for( var i = 0; i < projectData.Count(); i++ )
                {
                    IEnumerable<UserByProjectData> userByProject = await userQueries.GetUserByProject( projectData.ElementAt( i ).ProjectStudentId );
                    projectData.ElementAt( i ).BegDate = userByProject.ElementAt( 0 ).BegDate;
                    projectData.ElementAt( i ).EndDate = userByProject.ElementAt( 0 ).EndDate;
                    List<string> listGroups = await projectQueries.GetGroupsOfProject( projectData.ElementAt(i).ProjectStudentId );
                    List<string> semester = listGroups.FindAll( x => x.StartsWith( "S0" ) );
                    projectData.ElementAt( i ).Semester = semester[0];
                    List<string> sector = listGroups.FindAll( x => x.StartsWith( "IL" ) || x.StartsWith( "SR" ) );
                    if(sector.Count() >  0)
                    projectData.ElementAt(i).Sector = sector[0];
                    GroupData data = await groupQueries.GetIdSchoolByPeriodId( projectData.ElementAt( i ).ZoneId );
                    projectData.ElementAt( i ).SchoolId = data.ParentZoneId;
                    foreach( var e in userByProject )
                    {
                        //IEnumerable<GroupData> groupDatas = await groupQueries.GetAllGroupByTimedUser( e.TimedUserId );
                        projectData.ElementAt( i ).FirstName.Add( e.FirstName );
                        projectData.ElementAt( i ).LastName.Add( e.LastName );
                        projectData.ElementAt( i ).TimedUserId.Add( e.TimedUserId );
                        projectData.ElementAt( i ).UserId.Add( e.UserId );
                    }
                }
                return projectData;
            }
        }
        [HttpGet( "getProjectEval" )]
        public async Task<IEnumerable<AllProjectInfoData>> GetProjectsEvaluate(int idSchool)
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

                PeriodData periodData = await timedPeriodQueries.GetLastPeriodBySchool( idSchool );
                //Implementer check date period

                IEnumerable<AllProjectInfoData> projectData = await projectQueries.GetAllProjectTimedByJuryId( userId, periodData.ChildId );
                return projectData;
            }
        }

        // may we should rewrite the sql query
        public async Task<object> GetProjectSheet(int idx)
        {
            SqlDefaultDatabase db = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            int userId = _authenticationInfo.ActualUser.UserId;

            ProjectData pd;
            List<UserData> lst_usr_data;
            List<string> lst_grp;

            using(SqlStandardCallContext ctx = new SqlStandardCallContext())
            {
                ProjectQueries projectQueries = new ProjectQueries(ctx, db);

                pd = await projectQueries.GetDetailProject(idx);
                lst_usr_data = await projectQueries.GetAllUsersOfProject(idx);
                lst_grp = await projectQueries.GetGroupsOfProject(idx);

            }
            
            string name = pd.Name;
            // semesters of the project
            List<int> semesters = new List<int>();
            foreach (string elem in lst_grp) if (elem[0] == 'S' && (elem[1] == '0' || elem[1] == '1')) semesters.Add(int.Parse(elem.Substring(1, elem.Length-1)));
            semesters.Sort();
            string semester = String.Join(", ", semesters);

            // sector of the project
            string sector = lst_grp.Contains("SR") && lst_grp.Contains("IL") ? "IL - SR" : lst_grp.Contains("SR") ? "SR" : lst_grp.Contains("IL") ? "IL" : "";


            // download and encode the image in base64
            string logo = Convert.ToBase64String(new WebClient().DownloadData(pd.Logo));

            string slogan = pd.Slogan;
            string pitch = pd.Pitch;

            // check who is the leader
            string[] members = new string[lst_usr_data.Count-1];
            string leader = "";
            foreach(UserData usr in lst_usr_data)
            {
                if (usr.UserId == pd.LeaderId){leader = usr.FirstName + " " + usr.LastName;}
                else {members[Array.IndexOf(members, null)] = usr.FirstName + " " + usr.LastName;}
                
            }
            (string, string[]) team = (leader, members);

            // check the type for the field techno of background field and return the project
            if (pd.Type == "I")
            {
                string[] place = new string[2];
                place[0] = pd.ClassRoom;
                place[1] = pd.ForumNumber.ToString();
                string[] technos = pd.Technologies.ToArray();
                return new {project = new ProjectPiSheet(place, name, semester, sector, logo, slogan, pitch, team, technos), type = "I"};
            }
            else if (pd.Type == "H")
            {
                // download and encode the background in base64
                string background = Convert.ToBase64String(new WebClient().DownloadData(pd.Background ) );
                return new {project = new ProjectPfhSheet(name, semester, sector, logo, slogan, pitch, team, background), type = "H"};
            }
            else
            {
                return new {project = new ProjectSheet(name, semester, sector, logo, slogan, pitch, team,"None"), type = "None"};
            }
        }

        [HttpGet("getProjectSheet")]
        [AllowAnonymous]
        public async Task<IActionResult> SendProjectSheet(int idx)
        {
            return Ok(await GetProjectSheet(idx));
        }

        [HttpGet("GetAllSheet")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSheet(int schoolId, char projectType, int semester)
        {
            SqlDefaultDatabase db = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();

            using( SqlStandardCallContext ctx = new SqlStandardCallContext() )
            {
                ProjectQueries projectQueries = new ProjectQueries( ctx, db );
                TimedPeriodQueries timedPeriodQueries = new TimedPeriodQueries( ctx, db );


                PeriodData School = await timedPeriodQueries.GetLastPeriodBySchool( schoolId );

                IEnumerable<AllProjectInfoData> listProject;


                if( semester > 0 )
                {
                    if( projectType == 'I' || projectType == 'H' ) listProject = await projectQueries.GetAllTypeSchoolProject( School.ChildId, projectType, semester );
                    else listProject = await projectQueries.GetAllTypeSchoolProject( School.ChildId, semester );
                }
                else
                {
                    if( projectType == 'I' || projectType == 'H' ) listProject = await projectQueries.GetAllTypeSchoolProject( School.ChildId, projectType );
                    else listProject = await projectQueries.GetAllTypeSchoolProject( School.ChildId );

                }

                if( projectType == 'I' )
                {
                    List<ProjectPiSheet> projectsSheet = new List<ProjectPiSheet>();


                    foreach( var item in listProject )
                    {
                        string[] place = new string[2];
                        place[0] = item.ClassRoom;
                        place[1] = item.ForumNumber.ToString();

                        string[] members = new string[item.UsersData.Count - 1];
                        string leader = "";
                        foreach( UserData usr in item.UsersData )
                        {
                            if( usr.UserId == item.LeaderId ) { leader = usr.FirstName + " " + usr.LastName; }
                            else { members[Array.IndexOf( members, null )] = usr.FirstName + " " + usr.LastName; }

                        }
                        (string, string[]) team = (leader, members);


                        ProjectPiSheet projectPiSheet = new ProjectPiSheet( place, item.GroupName, item.Semester, item.Sector, item.Logo, item.Slogan, item.Pitch, team, item.Technologies.ToArray() );

                        projectsSheet.Add( projectPiSheet );
                    }

                    return Ok( projectsSheet );
                }
                else if( projectType == 'H' )
                {
                    List<ProjectPfhSheet> projectsSheet = new List<ProjectPfhSheet>();

                    foreach( var item in listProject )
                    {
                      
                        string[] members = new string[item.UsersData.Count - 1];
                        string leader = "";
                        foreach( UserData usr in item.UsersData )
                        {
                            if( usr.UserId == item.LeaderId ) { leader = usr.FirstName + " " + usr.LastName; }
                            else { members[Array.IndexOf( members, null )] = usr.FirstName + " " + usr.LastName; }

                        }
                        (string, string[]) team = (leader, members);

                        string background = Convert.ToBase64String( new WebClient().DownloadData( "https://drive.google.com/uc?id=143SNqM-rxFmDSrA7A2Wa29eu-gqhtdOn" ) );


                        ProjectPfhSheet projectPfhSheet = new ProjectPfhSheet( item.GroupName, item.Semester, "", item.Logo, item.Slogan, item.Pitch, team, background);

                        projectsSheet.Add( projectPfhSheet );
                    }

                    return Ok( projectsSheet );
                }
                else
                {
                    List<ProjectSheet> projectsSheet = new List<ProjectSheet>();

                    foreach( var item in listProject )
                    {

                        string[] members = new string[item.UsersData.Count - 1];
                        string leader = "";
                        foreach( UserData usr in item.UsersData )
                        {
                            if( usr.UserId == item.LeaderId ) { leader = usr.FirstName + " " + usr.LastName; }
                            else { members[Array.IndexOf( members, null )] = usr.FirstName + " " + usr.LastName; }

                        }
                        (string, string[]) team = (leader, members);



                        if( item.Type == "H" )
                        {
                            string background = Convert.ToBase64String( new WebClient().DownloadData( "https://drive.google.com/uc?id=143SNqM-rxFmDSrA7A2Wa29eu-gqhtdOn" ) );
                            ProjectPfhSheet projectPfhSheet = new ProjectPfhSheet( item.GroupName, item.Semester, "", item.Logo, item.Slogan, item.Pitch, team, background );
                            projectsSheet.Add( projectPfhSheet );

                        }
                        else
                        {
                            string[] place = new string[2];
                            place[0] = item.ClassRoom;
                            place[1] = item.ForumNumber.ToString();
                            ProjectPiSheet projectPiSheet = new ProjectPiSheet( place, item.GroupName, item.Semester, item.Sector, item.Logo, item.Slogan, item.Pitch, team, item.Technologies.ToArray() );
                            projectsSheet.Add( projectPiSheet );


                        }

                    }

                    return Ok( projectsSheet );
                }
            }


        }

        [HttpGet( "getAllTypeProjectsOfASchool" )]
        public async Task<IActionResult> GetAllTypeProjectsOfASchool(int idSchool, char type)
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

                PeriodData periodData = await timedPeriodQueries.GetLastPeriodBySchool( idSchool );
                //Implementer check date period

                TimedUserData timedUserData = await timedUserQueries.GetTimedUser( userId, periodData.ChildId );

                if( timedUserData == null )
                {
                    timedUserData = new TimedUserData
                    {
                        TimedUserId = 0
                    };
                }

                IEnumerable<AllProjectInfoData> projectData = await projectQueries.GetAllTypeProjectSpecificToSchoolWithTimedUserNote( periodData.ChildId,type,timedUserData.TimedUserId);
                for( var i = 0; i < projectData.Count(); i++ )
                {
                    IEnumerable<UserByProjectData> userByProject = await userQueries.GetUserByProject( projectData.ElementAt( i ).ProjectStudentId );
                    projectData.ElementAt( i ).BegDate = userByProject.ElementAt( 0 ).BegDate;
                    projectData.ElementAt( i ).EndDate = userByProject.ElementAt( 0 ).EndDate;

                    foreach( var e in userByProject )
                    {
                        IEnumerable<GroupData> groupDatas = await groupQueries.GetAllGroupByTimedUser( e.TimedUserId );
                        projectData.ElementAt( i ).FirstName.Add( e.FirstName );
                        projectData.ElementAt( i ).LastName.Add( e.LastName );
                        projectData.ElementAt( i ).TimedUserId.Add( e.TimedUserId );
                        projectData.ElementAt( i ).UserId.Add( e.UserId );
                    }
                }
                return Ok(projectData);
            }
        }

        [HttpPost( "noteProject" )]
        public async Task<IActionResult> NoteProject([FromBody] NoteProjectViewModel model )
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            TimedUserNoteProjectTable timedUserNoteProjectTable = _stObjMap.StObjs.Obtain<TimedUserNoteProjectTable>();
            TimedUserTable timedUserTable = _stObjMap.StObjs.Obtain<TimedUserTable>();
            EvaluatesTable evaluatesTable = _stObjMap.StObjs.Obtain<EvaluatesTable>();
            SqlDefaultDatabase db = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            PeriodServices periodServices = new PeriodServices();

            using( var ctx = new SqlStandardCallContext() )
            {
                ProjectQueries projectQueries = new ProjectQueries( ctx, db );
                TimedPeriodQueries timedPeriodQueries = new TimedPeriodQueries( ctx, db );
                UserQueries userQueries = new UserQueries( ctx, db );
                TimedUserQueries timedUserQueries = new TimedUserQueries( ctx, db );
                AclQueries aclQueries = new AclQueries( ctx, db );
                GroupQueries groupQueries = new GroupQueries( ctx, db );

                //Case Change Grade by Administration ====================================================================================================================
                if( model.User == ViewModels.TypeTimedUser.StaffMember )
                {
                    if( !await periodServices.CheckInPeriod( _stObjMap, _authenticationInfo ) )
                    {
                        Result result = new Result( Status.Unauthorized, "A la date d'aujourd'hui votre etablissement n'est dans une aucune periode" );
                        return this.CreateResult( result );
                    }

                    GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );

                    if( !await aclQueries.VerifyGrantLevelByUserId( 112, await aclQueries.GetAclIdBySchoolId( groupData.ParentZoneId), userId, Operator.SuperiorOrEqual ))
                    {
                        Result result = new Result( Status.Unauthorized, "Vous n'etes pas autorisé à utiliser cette fonctionnalité !" );
                        return this.CreateResult( result );
                    }

                    await evaluatesTable.EvaluateOrUpdateGradeProject( ctx, model.JuryId, model.ProjectId, model.Grade );
                    return Ok();
                }
                //=========================================================================================================================================================

                PeriodData periodData = await timedPeriodQueries.GetLastPeriodBySchool( model.SchoolId );
                                            
                TimedUserData timedUserData = await timedUserQueries.GetTimedUser( userId, periodData.ChildId );

                if(timedUserData == null )
                {
                    TimedUserStruct timedUser = await timedUserTable.CreateOrUpdateTimedUserAsyncWithType( ctx, Data.TypeTimedUser.Anon, periodData.ChildId, userId );
                    timedUserData = new TimedUserData
                    {
                        TimedUserId = timedUser.TimedUserId
                    };
                    await timedUserNoteProjectTable.AddOrUpdateNote( ctx, timedUserData.TimedUserId, model.ProjectId, model.Grade );
                    return Ok();
                }

                if( model.User == ViewModels.TypeTimedUser.Jury )
                {
                    await evaluatesTable.EvaluateOrUpdateGradeProject( ctx, model.JuryId, model.ProjectId, model.Grade );
                }
                else
                {
                    await timedUserNoteProjectTable.AddOrUpdateNote( ctx, timedUserData.TimedUserId, model.ProjectId, model.Grade );
                }

                return Ok();

            }

        }
        [HttpPost( "blockedProject" )]
        public async Task<IActionResult> BlockedProject( [FromBody] BlockedGradeViewModel model )
        {
            int userId = _authenticationInfo.ActualUser.UserId;
            EvaluatesTable evaluatesTable = _stObjMap.StObjs.Obtain<EvaluatesTable>();
            SqlDefaultDatabase db = _stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
            PeriodServices periodServices = new PeriodServices();


            using( var ctx = new SqlStandardCallContext() )
            {
                AclQueries aclQueries = new AclQueries( ctx, db );
                GroupQueries groupQueries = new GroupQueries( ctx, db );

                if( !await periodServices.CheckInPeriod( _stObjMap, _authenticationInfo ) )
                {
                    Result result = new Result( Status.Unauthorized, "A la date d'aujourd'hui votre etablissement n'est dans une aucune periode" );
                    return this.CreateResult( result );
                }

                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );

                if( !await aclQueries.VerifyGrantLevelByUserId( 112, await aclQueries.GetAclIdBySchoolId( groupData.ParentZoneId ), userId, Operator.SuperiorOrEqual ) )
                {
                    Result result = new Result( Status.Unauthorized, "Vous n'etes pas autorisé à utiliser cette fonctionnalité !" );
                    return this.CreateResult( result );
                }

                int idx = 0;
                foreach( var item in model.IndividualGrade )
                {
                    if(item.Value > 0) await evaluatesTable.BlockedProjectGrade( ctx, model.JurysId[idx], model.ProjectId, item.Value, true );
                    else await evaluatesTable.BlockedProjectGrade( ctx, model.JurysId[idx], model.ProjectId, true );
                    idx++;
                }
               
                return Ok();

            }
        }
    }
}
