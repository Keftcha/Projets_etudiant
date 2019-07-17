alter table CK.tUser
    add FirstName nvarchar(255) not null constraint DF_TEMP_1 default ('');

alter table CK.tUser
    add LastName nvarchar(255) not null constraint DF_TEMP_2 default ('');

alter table CK.tUser drop constraint DF_TEMP_1;
alter table CK.tUser drop constraint DF_TEMP_2;