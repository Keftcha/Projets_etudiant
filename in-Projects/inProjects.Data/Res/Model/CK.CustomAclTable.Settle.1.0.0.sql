declare @AclIdResult int,
        @SchoolId int,
        @GroupId int;

EXEC CK.sAclCreate 1, @AclIdResult output;

SELECT @SchoolId = SchoolId FROM IPR.tSchool ts WHERE ts.[Name] = 'IN''TECH';
UPDATE IPR.tSchool set AclPeriodManagementId = @AclIdResult where SchoolId = @SchoolId

SELECT @GroupId = GroupId FROM CK.tGroup g WHERE g.ZoneId = @SchoolId AND g.GroupName = 'Administration';
EXEC CK.sAclGrantSet 1,@AclIdResult,@GroupId,'',112
