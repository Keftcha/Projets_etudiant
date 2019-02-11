alter procedure viw.sGroupDelete
(
    @GroupId int
)
as
begin
    delete from viw.tMemberList where GroupId = @GroupId;
    delete from viw.tOwnGroup where GroupId = @GroupId ;
    delete from viw.tGroup where GroupId = @GroupId ;
    return 0;
end;