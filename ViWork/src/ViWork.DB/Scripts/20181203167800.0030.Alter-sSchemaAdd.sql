alter procedure viw.sSchemaAdd
(
    @SchemaName varchar(128),
	@GroupId int
)
as
begin
set transaction isolation level serializable;
	begin tran;

	if exists(select * from viw.tSchema s where s.SchemaName = @SchemaName)
	begin
		rollback;
		return 1;
	end;

	declare @SchemaId int;

    insert into viw.tSchema(SchemaName, GroupId) 
values(@SchemaName, @GroupId);
		select @SchemaId = scope_identity();
    insert into viw.tLnkSchema(SchemaId,  GroupId)
                           values(@SchemaId, @GroupId)
	commit;
    return 0;


end;