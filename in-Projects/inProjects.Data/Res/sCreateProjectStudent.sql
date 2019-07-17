-- SetupConfig: {"Requires": ["CK.sGroupCreate", "CK.sCKTraitFindOrCreate", "CK.sGroupUserAdd", "CK.sGroupGroupNameSet"]}
--
create procedure IPR.sCreateProjectStudent
(
    @ActorId int,
    @ProjectStudentId int output,
    @ZoneId INT,
    @Name nvarchar(128),

    @CKTraitContextId INT,
    @FindOnly BIT = 0,
    @TraitName VARCHAR(4096) = '',
    @CKTraitIdResult INT OUTPUT,

    @Logo nvarchar(126),
    @Slogan NVARCHAR(255),
    @Pitch TEXT,
    @LeaderId int = 0,
    @Type CHAR,
    @Background nvarchar(126) =''

)
as
begin
    --[beginsp]
    --<PreCreate />

     
 if exists(select * from CK.tGroup where GroupName = @Name and ZoneId = @ZoneId)
			BEGIN;
				THROW 51000, 'Le projet existe deja à cette periode !', 1;
			 END;


    if(@Type = 'I')
        BEGIN
            exec CK.sGroupCreate @ActorId, @ProjectStudentId OUTPUT, @ZoneId;
            EXEC CK.sCKTraitFindOrCreate @ActorId, @CKTraitContextId, @FindOnly, @TraitName, @CKTraitIdResult OUTPUT;
    
            insert into IPR.tProjectStudent VALUES (@ProjectStudentId, @Logo, @Slogan, @Pitch, @LeaderId, @Type, @CKTraitIdResult,null);
            --set @ProjectStudentId = SCOPE_IDENTITY()
            exec CK.sGroupUserAdd @ActorId, @ProjectStudentId, @LeaderId, 1
            exec CK.sGroupGroupNameSet @ActorId, @ProjectStudentId,@Name
        END
    ELSE
        BEGIN
            exec CK.sGroupCreate @ActorId, @ProjectStudentId OUTPUT, @ZoneId;
            insert into IPR.tProjectStudent VALUES (@ProjectStudentId, @Logo, @Slogan, @Pitch, @LeaderId, @Type, 0,@Background);
            exec CK.sGroupUserAdd @ActorId, @ProjectStudentId, @LeaderId, 1
            exec CK.sGroupGroupNameSet @ActorId, @ProjectStudentId,@Name
            SET  @CKTraitIdResult = 0
        END

    --<PostCreate revert />

    --[endsp]
end

