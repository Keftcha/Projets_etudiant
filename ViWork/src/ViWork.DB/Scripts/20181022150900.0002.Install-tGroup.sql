

create table viw.tGroup
(
	GroupId int identity(0,1),
	GroupName nvarchar(32),


	constraint Pk_tGroup primary key(GroupId),
	constraint CK_tGroup_GroupName check(GroupName <> N'')
);
insert into viw.tGroup(GroupName) values(left(convert(nvarchar(36), newid()),32));