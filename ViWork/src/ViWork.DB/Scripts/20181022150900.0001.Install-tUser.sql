create table viw.tUser
(
	UserId int identity(0,1),
	FirstName nvarchar(32),
	LastName nvarchar(32),
	Email nvarchar(32) not null,
	[Type] nvarchar(11) 


	constraint Pk_tUser primary key(UserId),
	constraint UK_tUser_email unique(Email),
	constraint CK_tUser_FirstName check(FirstName <> N''),
	constraint CK_tUser_LastName check(LastName <> N''),
	constraint CK_tUser_Type check([Type] in ('Prof','Eleve','Particulier'))
);
insert into viw.tUser(FirstName, LastName, Email, [Type]) values(left(convert(nvarchar(36), newid()),32),left(convert(nvarchar(36), newid()),32), left(convert(nvarchar(36), newid()),32), 'Particulier');