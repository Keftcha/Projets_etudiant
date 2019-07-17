using CK.SqlServer;
using CK.SqlServer.Setup;
using inProjects.Data.Data.ProjectStudent;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using inProjects.Data.Data.User;
using System;
using System.Net;

namespace inProjects.Data.Queries
{
    public class ProjectQueries
    {
        private ISqlConnectionController _controller;


        public ProjectQueries(ISqlCallContext ctx, SqlDefaultDatabase sqlDefaultDatabase )
        {
            _controller = ctx.GetConnectionController( sqlDefaultDatabase );
        }

        public async Task<ProjectUrlData> GetUrlByProject(int projectId )
        {
            ProjectUrlData result = await _controller.QuerySingleOrDefaultAsync<ProjectUrlData>(
                "SELECT * " +
                "FROM IPR.tProjectUrl p " +
                "WHERE p.ProjectId = @ProjectId",
                new { ProjectId = projectId } );
            return result;
        }

        public async Task<ProjectUserFavData> GetSPecificFavOfUser(int userId, int projectId)
        {
            ProjectUserFavData result = await _controller.QuerySingleOrDefaultAsync<ProjectUserFavData>(
                "SELECT * " +
                "FROM IPR.tUserFavProject uf " +
                "WHERE uf.UserId = @UserId AND uf.ProjectId = @ProjectId",
                new { ProjectId = projectId, UserId = userId } );
            return result;
        }

        public async Task<float> GetProjectGradeSpecificTimedUser( int projectId, int timedUserId )
        {
            float result = await _controller.QuerySingleOrDefaultAsync<float>(
                "select tu.Grade " +
                "from IPR.tTimedUserNoteProject tu " +
                "where tu.TimedUserId = @TimedUserId AND tu.StudentProjectId = @StudentProjectId",
                new { TimedUserId = timedUserId, StudentProjectId = projectId } );
            return result;
        }

        public async Task<IEnumerable<ProjectData>> GetAllProjectBySchool(int schoolId)
        {
            IEnumerable<ProjectData> result = await _controller.QueryAsync<ProjectData>(
                "SELECT *" +
                "FROM IPR.vProjectsDetails" +
                "WHERE ZoneId = @SchoolId",
                new { SchoolId = schoolId } );

            return result;
        }

        public async Task<List<ProjectData>> GetProjectsUser( int userId )
        {
            IEnumerable<ProjectData> result = await _controller.QueryAsync<ProjectData>(
                "select ps.ProjectStudentId, g.GroupName as [Name], ps.Logo from CK.tActorProfile ap" +
                " join IPR.tProjectStudent ps on ap.GroupId = ps.ProjectStudentId" +
                " join CK.tGroup g on g.GroupId = ps.ProjectStudentId where ap.ActorId = @UserId; ",
                new { UserId = userId } );

            return result.AsList();
        }

        public async Task<ProjectData> GetDetailProject( int idProject )
        {

            ProjectData project = await _controller.QueryFirstOrDefaultAsync<ProjectData>(
                        " SELECT ps.Logo, ps.Slogan, ps.Pitch, g.GroupName as [Name], pu.[Url], ps.LeaderId, ps.[Type],ps.Background,fi.ClassRoom,fi.ForumNumber" +
                        " from IPR.tProjectStudent ps" +
                        " join CK.tGroup g on g.GroupId = ps.ProjectStudentId" +
                        " left outer join IPR.tProjectUrl pu on pu.ProjectId = ps.ProjectStudentId" +
                        " LEFT OUTER JOIN IPR.tForumInfos fi on fi.ProjectId = ps.ProjectStudentId" + 
                        " where ProjectStudentId =  @ProjectId",
                        new { ProjectId = idProject } );

            //" SELECT ps.Logo, ps.Slogan, ps.Pitch, g.GroupName as [Name], pu.[Url], ps.LeaderId, ps.[Type], ps.Background, g.GroupName as Semester" +
            //" FROM IPR.tProjectStudent ps" +
            //" LEFT OUTER JOIN CK.tActorProfile ap on ap.ActorId = ps.ProjectStudentId" +
            //" LEFT OUTER JOIN CK.tGroup g on g.GroupId = ap.GroupId" +
            //" LEFT OUTER JOIN CK.tGroup g1 on g1.GroupId = ps.ProjectStudentId" +
            //" LEFT OUTER JOIN IPR.tForumInfos fi on fi.ProjectId = ps.ProjectStudentId" +
            //" LEFT OUTER JOIN IPR.tProjectUrl pu on  pu.ProjectId = ps.ProjectStudentId " +
            //" WHERE ps.ProjectStudentId = @ProjectId and g.GroupName <> g1.GroupName and g.GroupName like 'S0%'",
            //            new { ProjectId = idProject } );

            string technos = await _controller.QueryFirstOrDefaultAsync<string>(
                         "  SELECT ckt.TraitName as Technologies" +
                         " from IPR.tProjectStudent ps" +
                         " join CK.tGroup g on g.GroupId = ps.ProjectStudentId" +
                         " join CK.tCKTrait ckt on ps.TraitId = ckt.CKTraitId" +
                         " where ProjectStudentId =  @ProjectId",
                         new { ProjectId = idProject } );

            project.Technologies = technos.Split( ';' ).AsList();

            return project;
        }

        public async Task<List<UserData>> GetAllUsersOfProject( int idProject )
        {
             IEnumerable<UserData> result = await _controller.QueryAsync<UserData>(
                         "SELECT u.FirstName, u.LastName, u.UserId" +
                         " from CK.tGroup g join CK.tActorProfile ap on g.GroupId = ap.GroupId" +
                         " join CK.tUser u on u.UserId = ap.ActorId" +
                         " where g.GroupId = @ProjectId and ap.ActorId <> @ProjectId ",
                         new { ProjectId = idProject } );

            return result.AsList();
        }

        public async Task<List<string>> GetGroupsOfProject ( int idProject)
        {
            IEnumerable<string> result = await _controller.QueryAsync<string>( "SELECT g1.GroupName FROM IPR.tProjectStudent ps JOIN CK.vGroup g ON g.GroupId = ps.ProjectStudentId JOIN CK.tActorProfile ap ON ap.GroupId = g.GroupId JOIN CK.tUser u ON u.UserId = ap.ActorId LEFT OUTER JOIN CK.tActorProfile ap1 ON u.UserId = ap1.ActorId LEFT OUTER JOIN CK.vGroup g1 ON ap1.GroupId =  g1.GroupId WHERE g.IsZone = 0 AND g1.IsZone = 0 AND g.GroupName <> g1.GroupName AND g.GroupId = @ProjectId and ap1.ActorId <>0 GROUP BY g1.GroupName", new { ProjectId = idProject} );

            return result.AsList();
        }

        public async Task<bool> IsProjectFav(int idProject, int userId )
        {
            int project = await _controller.QuerySingleOrDefaultAsync<int>
                ( "SELECT * FROM IPR.tUserFavProject where UserId = @UserId and ProjectId = @ProjectId"
                , new { ProjectId = idProject, UserId = userId } );

            if(project == 0 )
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        public async Task<IEnumerable<AllProjectInfoData>> GetAllProject(int userId)
        {
            IEnumerable<AllProjectInfoData> result = await _controller.QueryAsync<AllProjectInfoData>( "SELECT ps.ProjectStudentId, ps.Logo, ps.Slogan, ps.Pitch, ps.LeaderId, ps.Type, t.TraitName, g.GroupName, g.ZoneId, isFav = uf.UserId FROM  IPR.tProjectStudent ps LEFT OUTER JOIN CK.tCKTrait t ON t.CKTraitId = ps.TraitId LEFT OUTER JOIN CK.tGroup g ON g.GroupId = ps.ProjectStudentId LEFT OUTER JOIN IPR.tUserFavProject uf ON uf.ProjectId = ps.ProjectStudentId WHERE uf.UserId = @UserId OR uf.UserId is null ;", new { UserId = userId } );
            return result;
        }

        public async Task<IEnumerable<AllProjectInfoData>> GetAllTypeProjectSpecificToSchoolWithTimedUserNote( int idSchool, char type, int timedUser)
        {

            IEnumerable<AllProjectInfoData> result = await _controller.QueryAsync<AllProjectInfoData>(
                          " select ps.ProjectStudentId,ps.Logo,g.GroupName, np.Grade" +
                          //" CASE WHEN np.Grade is null then - 1" +
                          //" ELSE np.Grade END as Grade " +
                          " from IPR.tProjectStudent ps" +
                          " left outer join IPR.tTimedUserNoteProject np on np.StudentProjectId = ps.ProjectStudentId  and np.TimedUserId = @TimedUser" +
                          " left outer join CK.tGroup g on ps.ProjectStudentId = g.GroupId" +
                          " where g.ZoneId = @SchoolId and ps.[Type] = @TypeProject ", new { SchoolId = idSchool, TypeProject = type, TimedUser = timedUser } );

            return result;
        }


        public async Task<IEnumerable<AllProjectInfoData>> GetAllTypeSchoolProject( int schoolId, char projectType, int semesterNumber )
        {
            string semester = "S0" + semesterNumber.ToString();

                IEnumerable<AllProjectInfoData> result = await _controller.QueryAsync<AllProjectInfoData>(
                "SELECT ps.ProjectStudentId, ps.Logo,ps.Pitch,ps.Slogan,ps.LeaderId,g1.GroupName,g.GroupName as Semester, fi.ClassRoom,fi.ForumNumber,ps.Type,ps.Background " +
                " FROM IPR.tProjectStudent ps" +
                " LEFT OUTER JOIN CK.tActorProfile ap on ap.ActorId = ps.ProjectStudentId" +
                " LEFT OUTER JOIN CK.tGroup g on g.GroupId = ap.GroupId" +
                " LEFT OUTER JOIN CK.tGroup g1 on g1.GroupId = ps.ProjectStudentId" +
                " LEFT OUTER JOIN IPR.tForumInfos fi on fi.ProjectId = ps.ProjectStudentId" +
                " WHERE g.ZoneId = @SchoolId and g.GroupName = @SemesterName and  ps.[Type] = @TypeProject and g.GroupName <> g1.GroupName AND g.GroupName like 'S0%'", new { SemesterName = semester, SchoolId = schoolId, TypeProject = projectType } );
                
            foreach( var item in result )
            {
                List<UserData> userData = new List<UserData>();

                item.UsersData = await GetAllUsersOfProject( item.ProjectStudentId );

                if( projectType == 'I' )
                {
                    string technos = await _controller.QueryFirstOrDefaultAsync<string>(
                            "  SELECT ckt.TraitName as Technologies" +
                           " from IPR.tProjectStudent ps" +
                           " join CK.tGroup g on g.GroupId = ps.ProjectStudentId" +
                           " join CK.tCKTrait ckt on ps.TraitId = ckt.CKTraitId" +
                          " where ProjectStudentId =  @ProjectId",
                           new { ProjectId = item.ProjectStudentId } );

                    item.Technologies = technos.Split( ';' ).AsList();
                    if( semesterNumber >= 3 )
                    {
                        List<string> groups = await GetGroupsOfProject( item.ProjectStudentId );
                        groups = groups.FindAll( x => x == "IL" || x == "SR" );
                        item.Sector = string.Join( ", ", groups.ToArray() );
                    }

                    

                }

                item.Logo = Convert.ToBase64String( new WebClient().DownloadData( item.Logo ) );
                item.Semester = semesterNumber.ToString();

            }

            return result;
        }

        public async Task<IEnumerable<AllProjectInfoData>> GetAllTypeSchoolProject( int schoolId, int semesterNumber )
        {
            string semester = "S0" + semesterNumber.ToString();

            IEnumerable<AllProjectInfoData> result = await _controller.QueryAsync<AllProjectInfoData>(
            "SELECT ps.ProjectStudentId, ps.Logo,ps.Pitch,ps.Slogan,ps.LeaderId,g1.GroupName,g.GroupName as Semester, fi.ClassRoom,fi.ForumNumber,ps.Type,ps.Background " +
            " FROM IPR.tProjectStudent ps" +
            " LEFT OUTER JOIN CK.tActorProfile ap on ap.ActorId = ps.ProjectStudentId" +
            " LEFT OUTER JOIN CK.tGroup g on g.GroupId = ap.GroupId" +
            " LEFT OUTER JOIN CK.tGroup g1 on g1.GroupId = ps.ProjectStudentId" +
            " LEFT OUTER JOIN IPR.tForumInfos fi on fi.ProjectId = ps.ProjectStudentId" +
            " WHERE g.ZoneId = @SchoolId and g.GroupName = @SemesterName and g.GroupName <> g1.GroupName AND g.GroupName like 'S0%'", new { SemesterName = semester, SchoolId = schoolId } );

            foreach( var item in result )
            {
                List<UserData> userData = new List<UserData>();

                item.UsersData = await GetAllUsersOfProject( item.ProjectStudentId );

                if( item.Type == "I" )
                {
                    string technos = await _controller.QueryFirstOrDefaultAsync<string>(
                            "  SELECT ckt.TraitName as Technologies" +
                           " from IPR.tProjectStudent ps" +
                           " join CK.tGroup g on g.GroupId = ps.ProjectStudentId" +
                           " join CK.tCKTrait ckt on ps.TraitId = ckt.CKTraitId" +
                          " where ProjectStudentId =  @ProjectId",
                           new { ProjectId = item.ProjectStudentId } );

                    item.Technologies = technos.Split( ';' ).AsList();
                    if( semesterNumber >= 3 )
                    {
                        List<string> groups = await GetGroupsOfProject( item.ProjectStudentId );
                        groups = groups.FindAll( x => x == "IL" || x == "SR" );
                        item.Sector = string.Join( ", ", groups.ToArray() );
                    }
                  
                }

                item.Logo = Convert.ToBase64String( new WebClient().DownloadData( item.Logo ) );
                item.Semester = semesterNumber.ToString();
            }

            return result;
        }

        public async Task<IEnumerable<AllProjectInfoData>> GetAllTypeSchoolProject( int schoolId, char projectType )
        {
            IEnumerable<AllProjectInfoData> result = await _controller.QueryAsync<AllProjectInfoData>(
               "SELECT ps.ProjectStudentId, ps.Logo,ps.Pitch,ps.Slogan,ps.LeaderId,g1.GroupName,g.GroupName as Semester, fi.ClassRoom,fi.ForumNumber,ps.Type,ps.Background " +
                " FROM IPR.tProjectStudent ps" +
                " LEFT OUTER JOIN CK.tActorProfile ap on ap.ActorId = ps.ProjectStudentId" +
                " LEFT OUTER JOIN CK.tGroup g on g.GroupId = ap.GroupId" +
                " LEFT OUTER JOIN CK.tGroup g1 on g1.GroupId = ps.ProjectStudentId" +
                " LEFT OUTER JOIN IPR.tForumInfos fi on fi.ProjectId = ps.ProjectStudentId" +
                " WHERE g.ZoneId = @SchoolId and ps.[Type] = @TypeProject and g.GroupName <> g1.GroupName AND g.GroupName like 'S0%'", new { SchoolId = schoolId, TypeProject = projectType } );

            foreach( var item in result )
            {
                List<UserData> userData = new List<UserData>();

                item.UsersData = await GetAllUsersOfProject( item.ProjectStudentId );
                int semester = int.Parse( item.Semester.Remove( 0, 2 ) );

                if( projectType == 'I' )
                {
                    string technos = await _controller.QueryFirstOrDefaultAsync<string>(
                            "  SELECT ckt.TraitName as Technologies" +
                           " from IPR.tProjectStudent ps" +
                           " join CK.tGroup g on g.GroupId = ps.ProjectStudentId" +
                           " join CK.tCKTrait ckt on ps.TraitId = ckt.CKTraitId" +
                          " where ProjectStudentId =  @ProjectId",
                           new { ProjectId = item.ProjectStudentId } );

                    item.Technologies = technos.Split( ';' ).AsList();
                    if( semester >= 3 )
                    {
                        List<string> groups = await GetGroupsOfProject( item.ProjectStudentId );
                        groups = groups.FindAll( x => x == "IL" || x == "SR" );
                        item.Sector = string.Join( ", ", groups.ToArray() );
                    }

                }

                item.Logo = Convert.ToBase64String( new WebClient().DownloadData( item.Logo ) );
                item.Semester = semester.ToString();

            }

            return result;
        }

        public async Task<IEnumerable<AllProjectInfoData>> GetAllTypeSchoolProject( int schoolId )
        {
            IEnumerable<AllProjectInfoData> result = await _controller.QueryAsync<AllProjectInfoData>(
               "SELECT ps.ProjectStudentId, ps.Logo,ps.Pitch,ps.Slogan,ps.LeaderId,g1.GroupName,g.GroupName as Semester, fi.ClassRoom,fi.ForumNumber,ps.Type,ps.Background " +
                " FROM IPR.tProjectStudent ps" +
                " LEFT OUTER JOIN CK.tActorProfile ap on ap.ActorId = ps.ProjectStudentId" +
                " LEFT OUTER JOIN CK.tGroup g on g.GroupId = ap.GroupId" +
                " LEFT OUTER JOIN CK.tGroup g1 on g1.GroupId = ps.ProjectStudentId" +
                " LEFT OUTER JOIN IPR.tForumInfos fi on fi.ProjectId = ps.ProjectStudentId" +
                " WHERE g.ZoneId = @SchoolId  and g.GroupName <> g1.GroupName AND g.GroupName like 'S0%'", new { SchoolId = schoolId } );

            foreach( var item in result )
            {
                List<UserData> userData = new List<UserData>();

                item.UsersData = await GetAllUsersOfProject( item.ProjectStudentId );
                int semester = int.Parse( item.Semester.Remove( 0, 2 ) );

                if( item.Type == "I" )
                {
                    string technos = await _controller.QueryFirstOrDefaultAsync<string>(
                            "  SELECT ckt.TraitName as Technologies" +
                           " from IPR.tProjectStudent ps" +
                           " join CK.tGroup g on g.GroupId = ps.ProjectStudentId" +
                           " join CK.tCKTrait ckt on ps.TraitId = ckt.CKTraitId" +
                          " where ProjectStudentId =  @ProjectId",
                           new { ProjectId = item.ProjectStudentId } );

                    item.Technologies = technos.Split( ';' ).AsList();
                    if( semester >= 3 )
                    {
                        List<string> groups = await GetGroupsOfProject( item.ProjectStudentId );
                        groups = groups.FindAll( x => x == "IL" || x == "SR" );
                        item.Sector = string.Join( ", ", groups.ToArray() );
                    }

                }

                item.Logo = Convert.ToBase64String( new WebClient().DownloadData( item.Logo ) );
                item.Semester = semester.ToString();

            }

            return result;
        }

        public async Task<IEnumerable<ProjectData>> GetAllProjectByForum( int forumId )
        {
            IEnumerable<ProjectData> result = await _controller.QueryAsync<ProjectData>(
                "SELECT * FROM IPR.vForumProjectInfos WHERE ForumId = @ForumId ORDER BY ForumNumber;",
                new { ForumId = forumId } );

            return result;
        }

        public async Task<IEnumerable<AllProjectInfoData>> GetAllProjectTimedByJuryId( int userId, int periodId )
        {
            IEnumerable<AllProjectInfoData> result = await _controller.QueryAsync<AllProjectInfoData>(
                "SELECT ps.ProjectStudentId,g.GroupName, e.Grade, ps.Logo,e.BlockedGrade as IsBlocked,e.JuryId" +
                " FROM IPR.tEvaluates e" +
                " JOIN CK.tActorProfile ap on ap.GroupId = e.JuryId" +
                " JOIN CK.tGroup g on g.GroupId = e.ProjectId" +
                " JOIN IPR.tProjectStudent ps on ps.ProjectStudentId = e.ProjectId" +
                " where ap.ActorId = @UserId and g.ZoneId = @ZoneId",
                new { UserId = userId ,ZoneId = periodId} );

            return result;
        }

        public async Task<List<ProjectInfosJuryData>> getAllProjectsGrade(int timePeriodId )
        {
            IEnumerable<ProjectInfosJuryData> result = await _controller.QueryAsync<ProjectInfosJuryData>(
                "SELECT g1.GroupName as ProjectName, g.GroupName as JuryName,g.GroupId as JuryId,e.BlockedGrade as IsBlocked,ps.ProjectStudentId,fi.ClassRoom,fi.ForumNumber," +
                " CASE WHEN e.Grade is null then - 1 " +
                " ELSE e.Grade END as Grade" +
                " FROM IPR.tEvaluates e" +
                " JOIN CK.tGroup g on e.JuryId = g.GroupId" +
                " JOIN IPR.tProjectStudent ps on ps.ProjectStudentId = e.ProjectId" +
                " JOIN CK.tGroup g1 on g1.GroupId = ps.ProjectStudentId" +
                " JOIN IPR.tForumInfos fi on fi.ProjectId = ps.ProjectStudentId" +
                " where g.ZoneId = @TimePeriodId" +
                " order by ps.ProjectStudentId", new { TimePeriodId = timePeriodId } );

            return result.AsList();
        }

        public async Task<List<ProjectForumResultData>> GetAllPublicsNoteByTimedPeriodId( int timePeriodId )
        {
            IEnumerable<ProjectForumResultData> result = await _controller.QueryAsync<ProjectForumResultData>(
                  "SELECT ps.ProjectStudentId AS ProjectId, g.GroupName as Name, SUM( np.Grade ) As Average, " +
                  "CountTotalVote =(SELECT Count(*) FROM IPR.tTimedUserNoteProject np JOIN CK.tGroup g on g.GroupId = np.StudentProjectId Where g.ZoneId = @TimePeriodId)" +
                  " FROM  IPR.tProjectStudent ps" +
                  " LEFT OUTER JOIN IPR.tTimedUserNoteProject np on ps.ProjectStudentId = np.StudentProjectId" +
                  " LEFT OUTER JOIN CK.tGroup g on g.GroupId = ps.ProjectStudentId" +
                  "  WHERE ps.Type = 'I' and g.ZoneId = @TimePeriodId" + 
                  " GROUP BY ps.ProjectStudentId, g.GroupName" +
                  " ORDER BY Average DESC"
                , new { TimePeriodId = timePeriodId } );

            return result.AsList();
        }

        public async Task<ProjectData> GetProjectIdByForumNumberAndPeriod(int forumNumber, int periodId)
        {
            ProjectData result = await _controller.QuerySingleOrDefaultAsync<ProjectData>( "SELECT ProjectStudentId = fi.ProjectId, g.GroupName FROM IPR.tForumInfos fi JOIN IPR.tProjectStudent ps ON ps.ProjectStudentId = fi.ProjectId JOIN CK.tGroup g ON g.GroupId = ps.ProjectStudentId WHERE g.ZoneId = @ZoneId AND fi.ForumNumber = @ForumNumber", new { ZoneId = periodId, ForumNumber = forumNumber } );
            return result;
        }

    }
}
