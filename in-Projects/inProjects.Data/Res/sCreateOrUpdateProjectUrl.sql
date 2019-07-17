 --SetupConfig: {}
--
CREATE PROCEDURE sCreateOrUpdateProjectUrl
(
    @ProjectUrlId INT = 0 OUTPUT,
    @ProjectId INT,
    @Url NVARCHAR(255),
    @UrlType NVARCHAR(40)

)
AS
BEGIN
    --[beginsp]
    --<PreCreate />


    IF EXISTS(SELECT * FROM IPR.tProjectUrl p WHERE p.ProjectId = @ProjectId AND p.UrlType = @UrlType)
        BEGIN
            UPDATE IPR.tProjectUrl SET [Url] = @Url WHERE ProjectId = @ProjectId AND UrlType = @UrlType;
        END

    ELSE
        BEGIN
            INSERT INTO IPR.tProjectUrl(ProjectId, [Url], UrlType) VALUES (@ProjectId, @Url, @UrlType);
            SET @ProjectUrlId = SCOPE_IDENTITY();
        END
    --<PostCreate revert />

    --[endsp]

END


