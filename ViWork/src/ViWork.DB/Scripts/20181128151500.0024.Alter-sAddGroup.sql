alter procedure viw.sGroupAdd
(
    @UserId   int,
    @GroupName varchar(128),
	@Status int
)
as
begin
set transaction isolation level serializable;
	begin tran;

	if exists(select * from viw.tGroup g where g.GroupName = @GroupName)
	begin
		rollback;
		return 1;
	end;
		
	declare @GroupId int;

    insert into viw.tGroup(GroupName ) 
values(@GroupName );
		select @GroupId = scope_identity();
    insert into viw.tOwnGroup(UserId,  GroupId)
                           values(@UserId, @GroupId)
	commit;
    return 0;


end;