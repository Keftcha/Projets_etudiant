alter procedure viw.sSchemaDelete
(
    @SchemaId int
)
as
begin
    delete from viw.tLnkSchema where SchemaId = @SchemaId;
    delete from viw.tSchema where SchemaId = @SchemaId;
    return 0;
end;