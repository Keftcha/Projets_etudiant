create procedure viw.sGroupUserDelete
(
    @GroupId int,
    @UserID int
)
as
begin
      delete from viw.tUserList where GroupId = @GroupId and UserId = @UserId;
    return 0;
end;