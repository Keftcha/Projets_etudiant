create view viw.vSchema
as
    select
        SchemaId = s.SchemaId,
        SchemaName = s.SchemaName,
        GroupId = s.GroupId
	
    from viw.tSchema s
    where s.SchemaId <> 0;