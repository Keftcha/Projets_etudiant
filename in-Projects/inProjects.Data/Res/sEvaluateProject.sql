 --SetupConfig: {}
--

CREATE PROCEDURE sEvaluateProject
(
    @JuryId INT,
    @ProjectId INT,
	@Grade float = null,
	@BegDate DATETIME2 = null,
    @BlockedNote BIT = 0
)
AS
BEGIN
--[beginsp]
--<PreCreate />
	
	if @BegDate is null
		BEGIN
            if not exists (SELECT * FROM IPR.tEvaluates where JuryId =@JuryId and ProjectId = @ProjectId and BlockedGrade = 1)
            BEGIN
		        UPDATE IPR.tEvaluates set Grade = @Grade, BlockedGrade = @BlockedNote where ProjectId = @ProjectId and JuryId = @JuryId
            END
		END
	else
		INSERT INTO IPR.tEvaluates VALUES(@JuryId, @ProjectId, @Grade, @BegDate,@BlockedNote)
	
--<PostCreate revert />

--[endsp]

END
