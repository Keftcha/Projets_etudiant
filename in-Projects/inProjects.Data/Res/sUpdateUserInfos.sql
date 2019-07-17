-- SetupConfig: {"Requires": ["CK.sActorEMailAdd"]}
--
create procedure sUpdateUserInfos
(
    @FirstName nvarchar(200),
    @LastName nvarchar(200),
    @PrimaryEmail nvarchar(200),
    @SecondaryEmail nvarchar(200),
    @UserId int,
    @Status INT = 0 OUTPUT
)
as
begin
    --[beginsp]
    --<PreCreate />
    declare @VerifyUser bit;

    if exists(SELECT * FROM CK.tActorEMail aem where aem.EMail =@PrimaryEmail and ActorId <> @UserId and IsPrimary = 1)
        begin
          set @Status =1
          COMMIT
          RETURN
        end

    if (@PrimaryEmail = @SecondaryEmail)
        begin
            set @Status = 2
             COMMIT
             RETURN
        end

    update CK.tUser set FirstName = @FirstName, LastName = @LastName where UserId = @UserId
    update CK.tActorEMail set EMail= @PrimaryEmail where ActorId = @UserId and IsPrimary = 1

    if exists(SELECT * FROM CK.tActorEMail   where ActorId = @UserId and IsPrimary = 0)
        begin
            update CK.tActorEMail set EMail= @SecondaryEmail where ActorId = @UserId and IsPrimary = 0
            COMMIT
            RETURN
        end

    EXEC CK.sActorEMailAdd @UserId,@UserId,@SecondaryEmail,0


    --<PostCreate revert />

    --[endsp]
end

