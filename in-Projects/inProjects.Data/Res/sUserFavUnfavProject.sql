 --SetupConfig: {}
--

CREATE PROCEDURE sUserFavUnfavProject
(
    @UserId INT,
    @ProjectId INT
)
AS
BEGIN
--[beginsp]
--<PreCreate />

IF EXISTS(SELECT * FROM IPR.tUserFavProject WHERE UserId = @UserId AND ProjectId = @ProjectId)
    BEGIN
        DELETE FROM IPR.tUserFavProject WHERE  UserId = @UserId AND ProjectId = @ProjectId;
    END
ELSE
    BEGIN
        INSERT INTO IPR.tUserFavProject(UserId, ProjectId) VALUES (@UserId, @ProjectId);
    END
--<PostCreate revert />

--[endsp]

END
