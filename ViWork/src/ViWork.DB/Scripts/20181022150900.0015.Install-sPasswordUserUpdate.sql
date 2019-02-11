create procedure viw.sPasswordUserUpdate
(
    @UserId   int,
    @Password varbinary(128)
)
as
begin
    update viw.tPasswordUser set [Password] = @Password where UserId = @UserId;
    return 0;
end;