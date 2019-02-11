create procedure viw.sGithubUserCreateOrUpdate
(
    @Email       nvarchar(64),
    @GithubId    int,
    @AccessToken varchar(64)

)
as
begin
	set transaction isolation level serializable;
	begin tran;

	if exists(select * from viw.tGithubUser u where u.GithubId = @GithubId)
	begin
		update viw.tGithubUser set AccessToken = @AccessToken where GithubId = @GithubId;
		commit;
		return 0;
	end;

    declare @userId int;
	declare @groupId int;
	select @userId = u.UserId from viw.tUser u where u.Email = @Email;

	if @userId is null
	begin
		insert into viw.tUser(Email) values(@Email);
		set @userId = scope_identity();
		insert into viw.tGroup( GroupName)
							values ('My group')
		select @groupId = SCOPE_IDENTITY()
		insert into viw.tOwnGroup(UserId,GroupId)
							values(@userId, @GroupId)

	end;
    
    insert into viw.tGithubUser(UserId,  GithubId,  AccessToken)
                         values(@userId, @GithubId, @AccessToken);
	commit;
    return 0;
end;