create table viw.tPasswordUser
(
	
	 UserId int not null,
    [Password] varbinary(128) not null,

    constraint PK_tPasswordUser primary key(UserId),
    constraint FK_tPasswordUser_UserId foreign key(UserId) references viw.tUser(UserId)

);

insert into viw.tPasswordUser(UserId, [Password]) values(0, convert(varbinary(128), newid()));