using CK.SqlServer;
using CK.SqlServer.Setup;
using inProjects.Data.ForumsPlan;
using System.Threading.Tasks;
using Dapper;
using System.Collections.Generic;
using inProjects.Data.Data;

namespace inProjects.Data.Queries
{
    public class ForumQueries
    {
        private ISqlConnectionController _controller;

        public ForumQueries( ISqlCallContext ctx, SqlDefaultDatabase sqlDefaultDatabase )
        {
            _controller = ctx.GetConnectionController( sqlDefaultDatabase );
        }

        public async Task<List<int>> SavePlan( Project[] plan )
        {
            List<int> results = new List<int>();

            for(int i = 0; i < plan.Length; i += 1 )
            {

                int res = await _controller.ExecuteAsync(
                    "update IPR.tForumInfos " +
                    "set ClassRoom = @Class," +
                    "CoordinatesX = @X," +
                    "CoordinatesY = @Y," +
                    "Width = @W," +
                    "Height = @H " +
                    "where ForumNumber = @ForumNumber",
                    new {
                        Class = plan[i].ClassRoom,
                        X = plan[i].PosX,
                        Y = plan[i].PosY,
                        W = plan[i].Width,
                        H = plan[i].Height,
                        ForumNumber = plan[i].ForumNumber
                    }
                );
                results.Add( res );
            }
            return results;
        }

        public async Task<IEnumerable<ForumData>> ForumInfoByJury(string juryName )
        {
            IEnumerable<ForumData> result = await _controller.QueryAsync<ForumData>( "SELECT Jury = g.GroupName, e.Date, Project = g1.GroupName FROM CK.tGroup g JOIN IPR.tEvaluates e ON e.JuryId = g.GroupId LEFT OUTER JOIN IPR.tProjectStudent ps ON ps.ProjectStudentId = e.ProjectId LEFT OUTER JOIN CK.tGroup g1 on g1.GroupId = e.ProjectId WHERE g.GroupName = @JuryName", new { JuryName = juryName } );
            return result;
        }

        public async Task<int> IsProjectJudgedByJury(int projectId, int juryId )
        {
            int result = await _controller.QuerySingleOrDefaultAsync<int>( "select JuryId from IPR.tEvaluates WHERE JuryId = @JuryId and ProjectId = @ProjectId", new { @JuryId = juryId, @ProjectId = projectId } );
            return result;
        }
    }
}
