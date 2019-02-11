create procedure viw.sUserUpdate
(
    @UserId int,
    @Email  nvarchar(64)
)
as
begin
    update viw.tUser
    set Email = @Email
    where UserId = @UserId;
    return 0;
end;