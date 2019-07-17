-- SetupConfig: {"Requires": [ "CK.sZoneCreate" ]}
--
create procedure IPR.sCreateSchool
(
    @ActorId int,
    @Name nvarchar(128),
    @SchoolId int output
)
as
begin
    --[beginsp]
    --<PreCreate />
   
    exec CK.sZoneCreate @ActorId, @SchoolId output; 
    insert into IPR.tSchool VALUES (@SchoolId,@Name,0);

    --<PostCreate revert />

    --[endsp]
end

