create procedure viw.sUserDelete
(
    @UserId int
)
as
begin
	delete from viw.tOwnGroup where UserId = @UserId;
    delete from viw.tPasswordUser where UserId = @UserId;
    delete from viw.tGithubUser where UserId = @UserId;
    delete from viw.tUser where UserId = @UserId;
    return 0;
end;