create table viw.tLnkSchema
(
	LId int identity(0,1),
	GroupId int not null, 
	SchemaId int not null,

	constraint Pk_tLnkSchema primary key(LId),
	constraint FK_tLnkSchema_tSchema foreign key(SchemaId) references viw.tSchema(SchemaId),
	constraint FK_tLnkSchemap_tGroup foreign key(GroupId) references viw.tGroup(GroupId)

);

GO

create unique index IX_tLnkSchema_GroupId on viw.tLnkSchema(GroupId) where GroupId <> 0;
create unique index IX_tLnkSchema_SchemaId on viw.tLnkSchema(SchemaId) where SchemaId <> 0;
insert into viw.tLnkSchema(GroupId, SchemaId) values(0,0);