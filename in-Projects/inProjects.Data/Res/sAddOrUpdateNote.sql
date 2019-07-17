-- SetupConfig: {}
--
create procedure IPR.sAddOrUpdateNote
(
    @TimedUserId int,
    @ProjectStudentId int,
    @Grade float
)
as
begin
    --[beginsp]
    --<PreCreate />
      
      if exists(select * from IPR.tTimedUserNoteProject tu where tu.TimedUserId = @TimedUserId AND tu.StudentProjectId = @ProjectStudentId)
            begin
                 update IPR.tTimedUserNoteProject set Grade = @Grade where TimedUserId = @TimedUserId AND StudentProjectId = @ProjectStudentId;
            end
      else
      insert into IPR.tTimedUserNoteProject VALUES (@TimedUserId, @ProjectStudentId, @Grade);

    --<PostCreate revert />

    --[endsp]
end

