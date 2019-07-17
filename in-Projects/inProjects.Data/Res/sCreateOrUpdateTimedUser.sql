-- SetupConfig: {"Requires":["CK.sGroupUserAdd"]}
--
create procedure IPR.sCreateOrUpdateTimedUser
(
    -- 0 is user anon, 1 is student, 2 is staffMember, 3 is jury
    @TypeUser INT,
    @TimePeriodId INT,
    @UserId INT,
    @TimedUserId INT OUTPUT,
    @Status INT OUTPUT
)
as
begin
    --[beginsp]
    --<PreCreate />
    declare @VerifyUser bit;

    if exists(select * from IPR.tTimedUser tu where tu.UserId = @UserId and tu.TimePeriodId = @TimePeriodId)
        begin
          set @VerifyUser =1;
          SET @TimedUserId = (select TimedUserId from IPR.tTimedUser tu where tu.UserId = @UserId and tu.TimePeriodId = @TimePeriodId);
        end

    if(@VerifyUser is null)
        begin
             insert into IPR.tTimedUser (TimePeriodId, UserId) VALUES (@TimePeriodId, @UserId);
             EXEC CK.sGroupUserAdd 1,@TimePeriodId,@UserId,1
             SET @TimedUserId = SCOPE_IDENTITY();
        end


    if(@TypeUser = 0)
        begin
             SET @Status = 0
        end
    else if(@TypeUser = 1)
        begin
            insert into IPR.tTimedStudent VALUES(@TimedUserId);
            SET @Status = 1
        end
    else if(@TypeUser = 2)
        begin
            insert into IPR.tTimedStaffMember VALUES(@TimedUserId);
             SET @Status = 2
        end
   else if(@TypeUser = 3)
        begin
            insert into IPR.tTimedJury VALUES(@TimedUserId);
            SET @Status = 3
        end

    --<PostCreate revert />

    --[endsp]
end

