 --SetupConfig: {"Requires": [ "CK.sGroupCreate", "CK.sGroupGroupNameSet" ]}
--

CREATE PROCEDURE sCreateEventSchool
(
    @Name VARCHAR(256),
	@BegDate DATETIME2,
	@EndDate DATETIME2,	
	@TimePeriodId INT,
	@ActorId INT,
	@EventId INT OUTPUT,
	@Status INT = 0 OUTPUT
)
AS
BEGIN
--[beginsp]
--<PreCreate />


	IF EXISTS(SELECT es.EventId FROM IPR.tEventSchool es join CK.tGroup g on g.GroupId = es.EventId where g.GroupName = @Name and g.ZoneId = @TimePeriodId)
			BEGIN
				set @Status = 1
				COMMIT 
				RETURN
			END

	  exec CK.sGroupCreate @ActorId, @EventId OUTPUT, @TimePeriodId
	  exec CK.sGroupGroupNameSet @ActorId, @EventId,@Name

	  insert into IPR.tEventSchool VALUES(@EventId, @BegDate,@EndDate)
	  
	
--<PostCreate revert />

--[endsp]

END
