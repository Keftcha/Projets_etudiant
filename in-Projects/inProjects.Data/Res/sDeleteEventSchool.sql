 --SetupConfig: {"Requires": [ "CK.sGroupDestroy" ]}
--

CREATE PROCEDURE sDeleteEventSchool
(
  @ActorId INT,
  @EventId INT,
  @ForceDestroy BIT,
  @Status INT = 0 OUTPUT
)
AS
BEGIN
--[beginsp]
--<PreCreate />


	IF NOT EXISTS(SELECT es.EventId FROM IPR.tEventSchool es join CK.tGroup g on g.GroupId = es.EventId where es.EventId = @EventId)
			BEGIN
				set @Status = 1
				COMMIT 
				RETURN
			END

	  delete from IPR.tEventSchool where EventId = @EventId
	  exec CK.sGroupDestroy @ActorId, @EventId,@ForceDestroy
	  
	
--<PostCreate revert />

--[endsp]

END
