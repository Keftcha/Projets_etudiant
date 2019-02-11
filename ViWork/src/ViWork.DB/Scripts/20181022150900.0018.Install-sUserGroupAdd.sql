create procedure viw.sUserGroupAdd
(
    @UserId   int,
    @GroupId int,
    @Status int
)
as
begin
set transaction isolation level serializable;
	begin tran;

	if exists(select * from viw.tMemberList m where m.UserId = @UserId)
	begin
		rollback;
		return 1;
	end;

      insert into viw.tMemberList(UserId,  GroupId)
                           values(@UserId, @GroupId)
	commit;
    return 0;


end;
