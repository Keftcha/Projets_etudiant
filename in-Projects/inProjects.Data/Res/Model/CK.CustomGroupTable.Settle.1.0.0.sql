DECLARE @GroupIdResult INT,
        @SchoolId INT;


EXEC CK.sGroupCreate 1, @GroupIdResult OUTPUT;
EXEC CK.sGroupGroupNameSet 1, @GroupIdResult, 'S01';

EXEC CK.sGroupCreate 1, @GroupIdResult OUTPUT;
EXEC CK.sGroupGroupNameSet 1, @GroupIdResult, 'S02';

EXEC CK.sGroupCreate 1, @GroupIdResult OUTPUT;
EXEC CK.sGroupGroupNameSet 1, @GroupIdResult, 'S03';

EXEC CK.sGroupCreate 1, @GroupIdResult OUTPUT;
EXEC CK.sGroupGroupNameSet 1, @GroupIdResult, 'S04';

EXEC CK.sGroupCreate 1, @GroupIdResult OUTPUT;
EXEC CK.sGroupGroupNameSet 1, @GroupIdResult, 'S05';

EXEC CK.sGroupCreate 1, @GroupIdResult OUTPUT;
EXEC CK.sGroupGroupNameSet 1, @GroupIdResult, 'S06';

EXEC CK.sGroupCreate 1, @GroupIdResult OUTPUT;
EXEC CK.sGroupGroupNameSet 1, @GroupIdResult, 'S07';

EXEC CK.sGroupCreate 1, @GroupIdResult OUTPUT;
EXEC CK.sGroupGroupNameSet 1, @GroupIdResult, 'S08';

EXEC CK.sGroupCreate 1, @GroupIdResult OUTPUT;
EXEC CK.sGroupGroupNameSet 1, @GroupIdResult, 'S09';

EXEC CK.sGroupCreate 1, @GroupIdResult OUTPUT;
EXEC CK.sGroupGroupNameSet 1, @GroupIdResult, 'S10';


EXEC CK.sGroupCreate 1, @GroupIdResult OUTPUT;
EXEC CK.sGroupGroupNameSet 1, @GroupIdResult, 'Teacher';

EXEC CK.sGroupCreate 1, @GroupIdResult OUTPUT;
EXEC CK.sGroupGroupNameSet 1, @GroupIdResult, 'Administration';

SELECT @SchoolId = SchoolId FROM IPR.tSchool ts WHERE ts.[Name] = 'IN''TECH';

EXEC CK.sGroupCreate 1, @GroupIdResult OUTPUT, @SchoolId;
EXEC CK.sGroupGroupNameSet 1, @GroupIdResult, 'Administration';

EXEC CK.sGroupCreate 1, @GroupIdResult OUTPUT, @SchoolId;
EXEC CK.sGroupGroupNameSet 1, @GroupIdResult, 'IL';

EXEC CK.sGroupCreate 1, @GroupIdResult OUTPUT, @SchoolId;
EXEC CK.sGroupGroupNameSet 1, @GroupIdResult, 'SR';
