
using Microsoft.AspNetCore.Http;
using System;
using CsvHelper;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using CK.Core;
using CK.Auth;
using System.Threading.Tasks;
using inProjects.Data;
using CK.SqlServer;
using inProjects.Data.Queries;
using CK.SqlServer.Setup;
using inProjects.Data.Data.Group;
using CK.DB.Actor.ActorEMail;
using System.Text;
using CK.DB.Auth;
using inProjects.Data.Data.TimedUser;
using inProjects.EmailJury;
using CK.DB.Zone;
using inProjects.Data.Data;
using inProjects.Data.Data.ProjectStudent;

namespace inProjects.WebApp.Services.CSV
{
    class CsvStudentMapping<T>
    {
        readonly IStObjMap _stObjMap;
        private readonly Emailer _emailSender;
        private List<UserList> _studentLists = new List<UserList>();

        public CsvStudentMapping()
            :base()
        {

        }

        public CsvStudentMapping( Emailer emailer)
        {
            _emailSender = emailer;
        }



        public async Task<List<UserList>> CSVReader(IFormFile path )
        {
            var streamResult = path.OpenReadStream();

            using( var reader = new StreamReader( streamResult, Encoding.UTF8 ) )
            using( var csv = new CsvReader( reader ) )
            {
                //csv.Configuration.Delimiter = ";";
                var records = csv.GetRecords<UserList>();
                var studentInfo = records.ToList();
                foreach(var e in studentInfo )
                {
                    string[] promotion = e.GroupName.Split( '-' );
                }
                return studentInfo;
            }

        }

        public async Task<bool> UserParser(List<UserList> studentList, IStObjMap stObjMap, IAuthenticationInfo authenticationInfo, string type)
        {
            using(var ctx = new SqlStandardCallContext() )
            {
                var sqlDatabase = stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
                var userTable = stObjMap.StObjs.Obtain<CustomUserTable>();
                var userTimedTable = stObjMap.StObjs.Obtain<TimedUserTable>();
                var groupTable = stObjMap.StObjs.Obtain<CustomGroupTable>();

                GroupQueries groupQueries = new GroupQueries( ctx, sqlDatabase );
                UserQueries userQueries = new UserQueries( ctx, sqlDatabase );

                int currentUserId = authenticationInfo.ActualUser.UserId;
                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( currentUserId );

                int zoneId = groupData.ZoneId;


                foreach( var element in studentList )
                {
                    string userName = Guid.NewGuid().ToString();
                    string firstName = element.FirstName;
                    string lastName = element.Name;
                    int timedUserType = 0;
                    string[] groupName = new string[1];
                    string mail = element.Mail;
                    int idUser = await CheckIfUserExists( stObjMap, authenticationInfo, mail, userName, firstName, lastName );

                    timedUserType = GetTimedUserType( type );
                    if( type == "student" ) groupName = element.GroupName.Split( '-' );                    
                    else if (type == "staffMember") groupName[0] = element.GroupName;
                    int groupId = await groupQueries.GetIdGroupByNameAndPeriod( groupName[0], zoneId );


                    TimedUserData timedUserData = await userQueries.CheckIfTimedUserExists( idUser, zoneId );

                    if (timedUserData == null )
                    {
                        await userTimedTable.CreateOrUpdateTimedUserAsync( ctx, timedUserType, zoneId, idUser );
                        groupTable.AddUser( ctx, currentUserId, groupId, idUser, true );
                        if( type == "student" && groupName[1] != "A" )
                        {
                            int sector = await groupQueries.GetIdGroupByNameAndPeriod( groupName[1], zoneId );
                            groupTable.AddUser( ctx, currentUserId, sector, idUser, true );
                        }
                    }
                    else
                    {
                        int isUserInGroup = await groupQueries.GetGroupByNamePeriodUser( groupName[0], idUser, zoneId );
                        if(isUserInGroup == 0 )
                        {
                            groupTable.AddUser( ctx, currentUserId, groupId, idUser, true );
                            if( type == "student" && groupName[1] != "A" )
                            {
                                int sector = await groupQueries.GetIdGroupByNameAndPeriod( groupName[1], zoneId );
                                groupTable.AddUser( ctx, currentUserId, sector, idUser, true );
                            }

                        }
                    }

                }
            }
            return true;
        }

        public async Task<int> CheckIfUserExists( IStObjMap stObjMap, IAuthenticationInfo authenticationInfo, string mail, string userName, string firstName, string lastName)
        {
            using( var ctx = new SqlStandardCallContext() )
            {
                var sqlDatabase = stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
                var userTable = stObjMap.StObjs.Obtain<CustomUserTable>();
                var actorEmail = stObjMap.StObjs.Obtain<ActorEMailTable>();
                var basic = stObjMap.StObjs.Obtain<IBasicAuthenticationProvider>();

                UserQueries userQueries = new UserQueries( ctx, sqlDatabase );

                int currentIdUser = authenticationInfo.ActualUser.UserId;

                int idUser = await userQueries.CheckEmail( mail );

                if(idUser != 0 )
                {
                    return idUser;
                }
                else
                {
                    string tempPwd = RandomPassword();
                    string subject = "Vous êtes invité à rejoindre la plateforme InProject";
                    string mailContent = "Afin de vous connectez a la plateforme InProject voici votre mot de passe provisoire: " + tempPwd + " il est conseillé de modifier ce mot de passe lors de votre première connection";
                    int newUserId = await userTable.CreateUserAsync( ctx, currentIdUser, userName, firstName, lastName );
                    await actorEmail.AddEMailAsync( ctx, 1, newUserId, mail, true, false );
                    await basic.CreateOrUpdatePasswordUserAsync( ctx, 1, newUserId, tempPwd );
                    //await _emailSender.SendMessage( mail, subject, mailContent );
                    return newUserId;
                }
            }
        }
      
        public string RandomPassword(int size = 8)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for(int i = 0; i < size; i++ )
            {
                ch = Convert.ToChar( Convert.ToInt32( Math.Floor( 26 * random.NextDouble() + 65 ) ) );
                builder.Append( ch );
            }

            return builder.ToString();

        }
        public async Task<List<T>> CSVReaderProjectNumber( IFormFile path )
        {
            var streamResult = path.OpenReadStream();

            using( var reader = new StreamReader( streamResult, Encoding.UTF8) )
            using( var csv = new CsvReader( reader ) )
            {

                //csv.Configuration.Delimiter = ";";
                var records = csv.GetRecords<T>();
                var projectInfo = records.ToList();
                return projectInfo;
            }
        }

        public async Task ForumNumberParser( IStObjMap stObjMap, List<ProjectNumbers> projectNumbers, IAuthenticationInfo authenticationInfo )
        {
            using( var ctx = new SqlStandardCallContext() )
            {
                var sqlDatabase = stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
                GroupQueries projectQueries = new GroupQueries( ctx, sqlDatabase );
                ForumInfosTable forumInfosTable = stObjMap.StObjs.Obtain<ForumInfosTable>();
                int userId = authenticationInfo.ActualUser.UserId;
                GroupData groupData = await projectQueries.GetIdSchoolByConnectUser( userId );

                foreach( ProjectNumbers projectNumber in projectNumbers )
                {
                    
                    int projectId = await projectQueries.GetSpecificIdGroupByZoneIdAndGroupName( groupData.ZoneId, projectNumber.ProjectName );
                    if( projectId == 0 ) Console.WriteLine("-----" + projectNumber.ProjectName );
                    await forumInfosTable.CreateForumInfo( ctx, projectId, "", -1, -1, 4, 3, projectNumber.ProjectNumber );
                   
                }
            }

        }

        internal async Task AssignProjectToJury( IStObjMap stObjMap, IAuthenticationInfo authenticationInfo, List<JuryInfos> juryInfos, string type )
        {
            using( var ctx = new SqlStandardCallContext() )
            {
                var sqlDatabase = stObjMap.StObjs.Obtain<SqlDefaultDatabase>();
                var groupTable = stObjMap.StObjs.Obtain<CustomGroupTable>();
                var userTimedTable = stObjMap.StObjs.Obtain<TimedUserTable>();
                EvaluatesTable evaluatesTable = stObjMap.StObjs.Obtain<EvaluatesTable>();

                int userId = authenticationInfo.ActualUser.UserId;
                GroupQueries groupQueries = new GroupQueries( ctx, sqlDatabase );
                GroupData groupData = await groupQueries.GetIdSchoolByConnectUser( userId );
                string userName = Guid.NewGuid().ToString();
                UserQueries userQueries = new UserQueries( ctx, sqlDatabase );
                TimedUserQueries timedUserQueries = new TimedUserQueries( ctx, sqlDatabase );
                ProjectQueries projectQueries = new ProjectQueries( ctx, sqlDatabase );
                TimedPeriodQueries timedPeriodQueries = new TimedPeriodQueries( ctx, sqlDatabase );
                ForumQueries forumQueries = new ForumQueries( ctx, sqlDatabase );
                //get the Begdate of the periode
                DateTime begDate = await timedPeriodQueries.GetBegDateOfPeriod( groupData.ZoneId );

                foreach( JuryInfos juryInfo in juryInfos )
                {
                    if( juryInfo.Mail == "six@hotmail.fr" )
                    {
                        Console.WriteLine( "ok" );
                    }
                    int enterpriseId = 0;
                    string groupName = "Jury " + juryInfo.Jury + "-" + begDate.Year+ "/" + begDate.Month;
                    int timedUserType = 0;
                    //Check if the jury group exists or not at a moment
                    int groupId = await groupQueries.GetSpecificIdGroupByZoneIdAndGroupName( groupData.ZoneId, groupName );
                    if( groupId == 0 )
                    {
                        groupId = await groupTable.CreateGroupAsync( ctx, userId, groupData.ZoneId );
                        await groupTable.Naming.GroupRenameAsync( ctx, userId, groupId, groupName );
                    }

                    //Check if the enterprise group exists or not at a moment
                    enterpriseId = await groupQueries.GetSpecificIdGroupByZoneIdAndGroupName( groupData.ZoneId, juryInfo.Entreprise );
                    if( enterpriseId == 0 )
                    {
                        enterpriseId = await groupTable.CreateGroupAsync( ctx, userId, groupData.ZoneId );
                        await groupTable.Naming.GroupRenameAsync( ctx, userId, enterpriseId, juryInfo.Entreprise );
                    }

                    //Check if the user exists
                    int idJury = await CheckIfUserExists( stObjMap, authenticationInfo, juryInfo.Mail, userName, juryInfo.Prenom, juryInfo.Nom );

                    await groupTable.AddUserAsync( ctx, userId, groupId, idJury,true );
                    await groupTable.AddUserAsync( ctx, userId, enterpriseId, idJury,true );

                    //get the timed user type
                    timedUserType = GetTimedUserType( type );

                    //check if the timed user exists
                    TimedUserData timedUserData = await userQueries.CheckIfTimedUserExists( idJury, groupData.ZoneId );
                    if(timedUserData == null )
                    {
                        await userTimedTable.CreateOrUpdateTimedUserAsync( ctx, timedUserType, groupData.ZoneId, idJury );

                    }
                    else
                    {
                        bool isJury = await timedUserQueries.IsJury( timedUserData.TimedUserId );
                        if(!isJury) await userTimedTable.CreateOrUpdateTimedUserAsync( ctx, timedUserType, groupData.ZoneId, idJury );
                    }

                    //get the project id by forumnumber
                    ProjectData projectId;
                    string groupName1 = "Vous êtes libre";
                    string groupName2 = "Vous êtes libre";
                    string groupName3 = "Vous êtes libre";
                    string groupName4 = "Vous êtes libre";
                    TimeSpan timeSpan1 = new TimeSpan( 13, 30, 0 );
                    TimeSpan timeSpan2 = new TimeSpan( 14, 15, 0 );
                    TimeSpan timeSpan3 = new TimeSpan( 15, 00, 0 );
                    TimeSpan timeSpan4 = new TimeSpan( 15, 45, 0 );

                    if( juryInfo.Groupe1 != 0 )
                    {
                        projectId = await projectQueries.GetProjectIdByForumNumberAndPeriod( juryInfo.Groupe1, groupData.ZoneId );
                        begDate = begDate.Date + timeSpan1;
                        if( await forumQueries.IsProjectJudgedByJury( projectId.ProjectStudentId, groupId ) == 0 )
                            await evaluatesTable.EvaluateOrUpdateGradeProject( ctx, groupId, projectId.ProjectStudentId, -1, begDate );
                        groupName1 = projectId.GroupName;
                    }

                    if(juryInfo.Groupe2 != 0 )
                    {
                        projectId = await projectQueries.GetProjectIdByForumNumberAndPeriod( juryInfo.Groupe2, groupData.ZoneId );
                        begDate = begDate.Date + timeSpan2;
                        if( await forumQueries.IsProjectJudgedByJury( projectId.ProjectStudentId, groupId ) == 0 )
                            await evaluatesTable.EvaluateOrUpdateGradeProject( ctx, groupId, projectId.ProjectStudentId, -1, begDate );
                        groupName2 = projectId.GroupName;

                    }

                    if(juryInfo.Groupe3 != 0 )
                    {
                        projectId = await projectQueries.GetProjectIdByForumNumberAndPeriod( juryInfo.Groupe3, groupData.ZoneId );
                        begDate = begDate.Date + timeSpan3;
                        if( await forumQueries.IsProjectJudgedByJury( projectId.ProjectStudentId, groupId ) == 0 )
                            await evaluatesTable.EvaluateOrUpdateGradeProject( ctx, groupId, projectId.ProjectStudentId, -1, begDate );
                        groupName3 = projectId.GroupName;

                    }
                    if(juryInfo.Groupe4 != 0 )
                    {
                        projectId = await projectQueries.GetProjectIdByForumNumberAndPeriod( juryInfo.Groupe4, groupData.ZoneId );
                        begDate = begDate.Date + timeSpan4;
                        if( await forumQueries.IsProjectJudgedByJury( projectId.ProjectStudentId, groupId ) == 0 )
                            await evaluatesTable.EvaluateOrUpdateGradeProject( ctx, groupId, projectId.ProjectStudentId, -1, begDate );
                        groupName4 = projectId.GroupName;

                    }

                    IEnumerable<ForumData> infoMail = await forumQueries.ForumInfoByJury( groupName );
                    int nbProject = infoMail.Count();

                    string subject = "Votre participation au Forum PI D'IN'TECH";
                    string mailContent = @"
            <table align = 'center' border='0' cellpadding='0' cellspacing='0' height='100%' width='100%' id='bodyTable'>
  
                            </tr>
                            <tr>
                                <td valign = 'top' id='templateBody'><table border = '0' cellpadding='0' cellspacing='0' width='100%' class='mcnTextBlock' style='min-width:100%;'>
    <tbody class='mcnTextBlockOuter'>
        <tr>
            <td valign = 'top' class='mcnTextBlockInner' style='padding-top:9px;'>
              	<!--[if mso]>
				<table align = 'left' border='0' cellspacing='0' cellpadding='0' width='100%' style='width:100%;'>
				<tr>
				<![endif]-->
			    
				<!--[if mso]>
				<td valign = 'top' width='600' style='width:600px;'>
				<![endif]-->
                <table align = 'left' border='0' cellpadding='0' cellspacing='0' style='max-width:100%; min-width:100%;' width='100%' class='mcnTextContentContainer'>
                    <tbody><tr>
                        
                        <td valign = 'top' class='mcnTextContent' style='padding-top:0; padding-right:18px; padding-bottom:9px; padding-left:18px;'>
                        
                            <h1>Bonjour " + juryInfo.Prenom + " " + juryInfo.Nom + @"</h1>

<p>Vous participez au forum des projets informatique de l'école IN'TECH le " +begDate.Day + "/0" +begDate.Month + "/" +begDate.Year + @".</p>

<p>Vous appartenez au jury '" + groupName + @"'.<br>
Au cours de cet évènement vous aurez l'occasion de juger les projets suivants:&nbsp;</p>

<ul>
	<li>" + groupName1 + " à " +  timeSpan1 + @"</li>
	<li>" + groupName2 + " à " + timeSpan2 + @"</li>
	<li>" + groupName3 + " à " + timeSpan3 + @"</li>
	<li>" + groupName4 + " à " + timeSpan4 + @"</li>
</ul>
Nous vous remercions de l’intérêt que vous portez à nos évènements.<br>
Bonne continuation.
                        </td>
                    </tr>
                </tbody></table>
				<!--[if mso]>
				</td>
				<![endif]-->
                
				<!--[if mso]>
				</tr>
				</table>
				<![endif]-->
            </td>
        </tr>
    </tbody>
</table></td>
                            </tr>
                            <tr>
                                <td valign = 'top' id= 'templateFooter' >
        <td align = 'center' style='padding-left:9px;padding-right:9px;'>
            <table border = '0' cellpadding='0' cellspacing='0' width='100%' style='min-width:100%;' class='mcnFollowContent'>
                <tbody><tr>
                    <td align = 'center' valign='top' style='padding-top:9px; padding-right:9px; padding-left:9px;'>
                        <table align = 'center' border='0' cellpadding='0' cellspacing='0'>
                            <tbody><tr>
                                <td align = 'center' valign='top'>
                                    <!--[if mso]>
                                    <table align = 'center' border='0' cellspacing='0' cellpadding='0'>
                                    <tr>
                                    <![endif]-->

                                </td>
                            </tr>
                        </tbody></table>
                    </td>
                </tr>
            </tbody></table>
        </td>
    </tr>
</tbody></table>

            </td>
        </tr>
    </tbody>
</table>
    <tbody class='mcnDividerBlockOuter'>
        <tr>
            <td class='mcnDividerBlockInner' style='min-width: 100%; padding: 10px 18px 25px;'>
                <table class='mcnDividerContent' border='0' cellpadding='0' cellspacing='0' width='100%' style='min-width: 100%;border-top: 2px solid #EEEEEE;'>
                    <tbody><tr>
                        <td>
                            <span></span>
                        </td>
                    </tr>
                </tbody></table>
<!--            
                <td class='mcnDividerBlockInner' style='padding: 18px;'>
                <hr class='mcnDividerContent' style='border-bottom-color:none; border-left-color:none; border-right-color:none; border-bottom-width:0; border-left-width:0; border-right-width:0; margin-top:0; margin-right:0; margin-bottom:0; margin-left:0;' />
-->
            </td>
        </tr>
    </tbody>
</table>
    <tbody class='mcnTextBlockOuter'>
        <tr>
            <td valign = 'top' class='mcnTextBlockInner' style='padding-top:9px;'>
			    
				<!--[if mso]>
				<td valign = 'top' width='600' style='width:600px;'>
				<![endif]-->
                <table align = 'left' border='0' cellpadding='0' cellspacing='0' style='max-width:100%; min-width:100%;' width='100%' class='mcnTextContentContainer'>
                    <tbody><tr>
                        
                        <td valign = 'top' class='mcnTextContent' style='padding-top:0; padding-right:18px; padding-bottom:9px; padding-left:18px;'>
                        
                            <br>
<br>
<strong>Notre adresse:</strong><br>
74 bis Avenue Maurice Thorez, 94200 Ivry-sur-Seine<br>
Metro 7: Pierre et Marie Curie<br>
<br>
IN'TECH Paris
                        </td>
                    </tr>
                </tbody></table>
				<!--[if mso]>
				</td>
				<![endif]-->
                
				<!--[if mso]>
				</tr>
				</table>
				<![endif]-->
            </td>
        </tr>
    </tbody>
</table></td>
                            </tr>
                        </table>
                        <!--[if (gte mso 9)|(IE)]>
                        </td>
                        </tr>
                        </table>
                        <![endif]-->
                        <!-- // END TEMPLATE -->
                    </td>
                </tr>
            </table>
        </center>
    </body>
</html>";

                    await _emailSender.SendMessage( juryInfo.Mail, subject, mailContent );



                }
            }
        }

        public int GetTimedUserType(string type )
        {
            if( type == "student" ) return 1;
            if( type == "staffMember" ) return 2;
            if( type == "jury" ) return 3;
            return 0;

        }

    }

}
