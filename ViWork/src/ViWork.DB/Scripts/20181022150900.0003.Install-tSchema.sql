create table viw.tSchema
(
	SchemaId int identity(0,1),
	SchemaName nvarchar(32) not null,
	GroupId int not null,

	constraint Pk_tSchema primary key(SchemaId),
	constraint CK_tSchema_GroupName check(SchemaName <> N''),
);
insert into viw.tSchema(SchemaName, GroupId) values(left(convert(nvarchar(36), newid()),32),0);