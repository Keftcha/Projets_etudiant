 --SetupConfig: {}
--

CREATE PROCEDURE sCreateForumInfo
(
    @ProjectId INT,
    @ClassRoom VARCHAR(5),
    @CoordinatesX INT,
    @CoordinatesY INT,
    @Width INT,
    @Height INT,
    @ForumNumber INT
)
AS BEGIN
    --[beginsp]
    --<PreCreate />

    INSERT INTO IPR.tForumInfos VALUES (@ProjectId, @ClassRoom, @CoordinatesX, @CoordinatesY, @Width, @Height, @ForumNumber);

    --<PostCreate revert />

    --[endsp]
END
