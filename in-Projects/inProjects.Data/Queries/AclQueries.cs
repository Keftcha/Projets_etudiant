using CK.SqlServer;
using CK.SqlServer.Setup;
using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace inProjects.Data.Queries
{

    public class AclQueries
    {
        private ISqlConnectionController _controller;

        public AclQueries( ISqlCallContext ctx, SqlDefaultDatabase sqlDefaultDatabase )
        {
            _controller = ctx.GetConnectionController( sqlDefaultDatabase );
        }

        public async Task<bool> VerifyGrantLevelByUserId(int grantLevel, int aclId, int userId, Operator op )
        {
            int result = 0;

            if( op == Operator.SuperiorOrEqual )
            {
                result = await _controller.QuerySingleOrDefaultAsync<int>
                    ( "select * from CK.vAclActor a where a.ActorId = @ActorId and a.AclId = @Acl and a.GrantLevel >= @GrantLevel",
                             new { ActorId = userId, Acl = aclId, GrantLevel = grantLevel } );

            }else if(op == Operator.InferiorOrEqual )
            {
                result = await _controller.QuerySingleOrDefaultAsync<int>
                   ( "select * from CK.vAclActor a where a.ActorId = @ActorId and a.AclId = @Acl and a.GrantLevel <= @GrantLevel",
                            new { ActorId = userId, Acl = aclId, GrantLevel = grantLevel } );

            }else if(op == Operator.Equal )
            {
                result = await _controller.QuerySingleOrDefaultAsync<int>
                   ( "select * from CK.vAclActor a where a.ActorId = @ActorId and a.AclId = @Acl and a.GrantLevel = @GrantLevel",
                            new { ActorId = userId, Acl = aclId, GrantLevel = grantLevel } );

            }else if(op == Operator.StrictlyInferior )
            {
                result = await _controller.QuerySingleOrDefaultAsync<int>
                   ( "select * from CK.vAclActor a where a.ActorId = @ActorId and a.AclId = @Acl and a.GrantLevel < @GrantLevel",
                            new { ActorId = userId, Acl = aclId, GrantLevel = grantLevel } );

            }else if(op == Operator.StrictlySuperior )
            {
                result = await _controller.QuerySingleOrDefaultAsync<int>
                   ( "select * from CK.vAclActor a where a.ActorId = @ActorId and a.AclId = @Acl and a.GrantLevel > @GrantLevel",
                            new { ActorId = userId, Acl = aclId, GrantLevel = grantLevel } );

            }


            if(result == 0 )
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }

        public async Task<int> GetAclIdBySchoolId (int idSchool )
        {
            return await _controller.QuerySingleAsync<int>( "SELECT AclPeriodManagementId FROM IPR.tSchool where SchoolId = @IdSchool ", new { IdSchool = idSchool } );
        }
    }

    public enum Operator
    {
        SuperiorOrEqual,
        InferiorOrEqual,
        Equal,
        StrictlySuperior,
        StrictlyInferior
    }
}
