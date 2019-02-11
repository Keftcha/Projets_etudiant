create procedure viw.sGroupNameUpdate
(
    @GroupId int,
    @GroupName  nvarchar(64)
)
as
begin
    update viw.tGroup
    set GroupName  = @GroupName  
    where GroupId = @GroupId ;
    return 0;
end;